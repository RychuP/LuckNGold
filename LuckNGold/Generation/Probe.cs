using GoRogue.MapGeneration.ContextComponents;

namespace LuckNGold.Generation;

internal class Probe
{
    // TODO these 3 are just for debugging class... delete at some point
    public Rectangle Bounds => _bounds;
    public List<Point> SideEdges => _sideEdges;
    public List<Point> OuterEdge => _outerEdge;

    // edges perpendicular to the probe's direction
    readonly List<Point> _sideEdges = [];

    // furthest edge in the probe's direction
    readonly List<Point> _outerEdge = [];

    // direction in which this probe was sent from the room
    readonly Direction _direction;

    // the room of which surrounding space is being probed
    readonly Room _room;

    // probe area with added border space for walls
    Rectangle _bounds;

    /// <summary>
    /// Current width of the probe area.
    /// </summary>
    public int Width => Area.Width;

    /// <summary>
    /// Current height of the probe area.
    /// </summary>
    public int Height => Area.Height;

    /// <summary>
    /// Length of the path to the probe area.
    /// </summary>
    public int PathLength { get; init; }

    // area covered by the probe -> gets reduced as the search goes on
    Rectangle _area;
    public Rectangle Area
    {
        get => _area;
        private set
        {
            if (value.Width < Room.MinOddSize || value.Height < Room.MinOddSize)
            {
                throw new ProbeException("Area width or height is too small.");
            }

            _area = value;
            OnAreaChanged(value);
        }
    }

    public Probe(Room room, Direction direction, int pathLength)
    {
        _direction = direction;
        _room = room;
        PathLength = pathLength;

        // calculate initial probe area size
        var wallSize = room.GetWallSize(direction);
        var size = wallSize.IsOdd() ? Room.MaxOddSize : Room.MaxEvenSize;

        // calculate position for the area
        (int x, int y) = room.GetAdjacentAreaPosition(direction, pathLength, size);

        // create area
        Area = new(x, y, size, size);
    }

    /// <summary>
    /// Checks if there is enough space in the probe's area to place a room.
    /// </summary>
    /// <param name="rooms">List of all the rooms created so far.</param>
    /// <param name="mapBounds">Bounds of the map.</param>
    /// <exception cref="ProbeException">Thrown when the area gets too small.</exception>
    public bool CheckArea(List<Room> rooms, Rectangle mapBounds)
    {
        // check area is within bounds and reduce it if not
        if (!mapBounds.Contains(_bounds))
            Area = Rectangle.GetIntersection(mapBounds, _bounds).Expand(-1, -1);

        // check for overlap with other rooms
        foreach (var room in rooms)
        {
            // skip parent room
            if (room == _room) 
                continue;

            // skip rooms with no intersections
            if (!room.Bounds.Intersects(_bounds))
                continue;

            // find the smallest possible area where the probe bounds
            // don't intersect the bounds of the room
            bool sideEdgesIntersect = CheckRoomContainsPoints(room, _sideEdges);
            bool outerEdgeIntersects = CheckRoomContainsPoints(room, _outerEdge);

            // both edges intersect
            if (sideEdgesIntersect && outerEdgeIntersects)
            {
                // save the current area for comparison later
                var backupArea = Area;

                // shrink area twice from both outer edge and side edges
                Rectangle areaReducedOnOuterEdge = Rectangle.Empty;
                try
                {
                    ShrinkAreaOnOuterEdgeOnly(room.Bounds);
                    areaReducedOnOuterEdge = Area;
                }
                catch (ProbeException) { }

                Area = backupArea;
                Rectangle areaReducedOnSideEdges = Rectangle.Empty;
                try
                {
                    ShrinkAreaOnSideEdgesOnly(room.Bounds);
                    areaReducedOnSideEdges = Area;
                }
                catch (ProbeException) { }

                if (areaReducedOnOuterEdge == Rectangle.Empty &&
                    areaReducedOnSideEdges == Rectangle.Empty)
                {
                    throw new ProbeException("Two attempts at reducing the probe area failed. " +
                        "Not enough space to place a room.");
                }

                // compare the resulting areas and select the bigger one
                Area = areaReducedOnOuterEdge.Area >= areaReducedOnSideEdges.Area ?
                    areaReducedOnOuterEdge : areaReducedOnSideEdges;

            }
            // only one edge intersects (easier case)
            else
            {
                // shrink area from the intersecting edge only
                if (outerEdgeIntersects)
                    ShrinkAreaOnOuterEdgeOnly(room.Bounds);
                else
                    ShrinkAreaOnSideEdgesOnly(room.Bounds);
            }
        }

        return true;
    }

    // checks if room contains any of given points and if 
    static bool CheckRoomContainsPoints(Room room, List<Point> points)
    {
        foreach (var point in points)
        {
            if (room.Bounds.Contains(point))
            {
                return true;
            }
        }

        return false;
    }

    // keeps moving the side edges inwards until no more intersections are found
    // or the exception is thrown
    void ShrinkAreaOnSideEdgesOnly(Rectangle roomBounds)
    {
        while (roomBounds.Intersects(_bounds))
            MoveSideEdgesInwards();
    }

    // keeps moving the outer edge inwards until no more intersections are found
    // or the exception is thrown
    void ShrinkAreaOnOuterEdgeOnly(Rectangle roomBounds)
    {
        while (roomBounds.Intersects((_bounds)))
            MoveOuterEdgeInwards();
    }

    // reduces the size of the probe area by moving side edges inwards
    // while keeping the position of the area centered with reference
    // to the room that sent the probe out
    void MoveSideEdgesInwards()
    {
        var delta = _direction.Type switch
        {
            Direction.Types.Up => (1, 0),
            Direction.Types.Down => (1, 0),
            _ => (0, 1),
        };

        ReduceRectangleSize(Area.Width, Area.Width - 2, Area.Height - 2,
            Area.Height, delta);
    }

    // reduces the size of the probe area by moving outer edge inwards
    // while keeping the position of the area centered with reference
    // to the room that sent the probe out
    void MoveOuterEdgeInwards()
    {
        var delta = _direction.Type switch
        {
            Direction.Types.Up => (0, 1),
            Direction.Types.Left => (1, 0),
            _ => (0, 0),
        };

        ReduceRectangleSize(Area.Width - 1, Area.Width, Area.Height,
            Area.Height - 1, delta);
    }

    // helper function for the edge moving methods
    void ReduceRectangleSize(int width1, int width2,
        int height1, int height2, Point delta)
    {
        var pos = Area.Position + delta;
        var width = _direction.IsHorizontal() ? width1 : width2;
        var height = _direction.IsHorizontal() ? height1 : height2;

        // exception will be thrown from the property if the width or height is too small
        Area = new Rectangle(pos.X, pos.Y, width, height);
    }

    void OnAreaChanged(Rectangle newArea)
    {
        _bounds = newArea.Expand(1, 1);

        var perpendicularDirections = _direction.GetPerpendicularDirections();

        // save side boundary points
        _sideEdges.Clear();
        _sideEdges.AddRange(_bounds.PositionsOnSide(perpendicularDirections[0]));
        _sideEdges.AddRange(_bounds.PositionsOnSide(perpendicularDirections[1]));

        // save outer edge points
        _outerEdge.Clear();
        _outerEdge.AddRange(_bounds.PositionsOnSide(_direction));
    }
}