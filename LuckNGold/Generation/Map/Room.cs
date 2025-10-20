namespace LuckNGold.Generation.Map;

/// <summary>
/// Simple, square, walled space with four available exits placed centrally on each wall.
/// Basic building block of the dungeon map.
/// </summary>
partial class Room
{
    /// <summary>
    /// Inner area of the room.
    /// </summary>
    public Rectangle Area { get; }

    /// <summary>
    /// Bounds of the room that includes inner area and walls around it.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Width of the room.
    /// </summary>
    public int Width => Area.Width;

    /// <summary>
    /// Height of the room.
    /// </summary>
    public int Height => Area.Height;

    /// <summary>
    /// Position of the room.
    /// </summary>
    public Point Position => Area.Position;

    /// <summary>
    /// Wall directions.
    /// </summary>
    public static Direction[] Directions =>
        AdjacencyRule.Cardinals.DirectionsOfNeighborsCache;

    /// <summary>
    /// Number of rooms between this room and the exit room of the section.
    /// </summary>
    public int DistanceToSectionExit { get; set; }

    /// <summary>
    /// <see cref="RoomPath"/> that this room belongs to.
    /// </summary>
    public RoomPath Path { get; private set; }

    readonly List<Entity> _contents = [];
    /// <summary>
    /// List of entities placed in the room.
    /// </summary>
    public IReadOnlyList<Entity> Contents { get => _contents; }

    Section? _section = null;
    /// <summary>
    /// Section of the dungeon this room is part of.
    /// </summary>
    public Section? Section
    {
        get => _section;
        set
        {
            if (_section != null)
                throw new InvalidOperationException("Section should not change once allocated.");
            _section = value;
        }
    }

    /// <summary>
    /// Initializes and instance of the <see cref="Room"/> class with the given parameters.
    /// </summary>
    /// <param name="x">X coordinate of the position of the room on the map.</param>
    /// <param name="y">Y coordinate of the position of the room on the map.</param>
    /// <param name="width">Width of the room.</param>
    /// <param name="height">Height of the room.</param>
    /// <param name="parent">Path this room belongs to.</param>
    public Room(int x, int y, int width, int height, RoomPath parent)
    {
        Area = new Rectangle(x, y, width, height);
        Bounds = Area.Expand(1, 1);
        Path = parent;
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Room"/> class with the given parameters.
    /// </summary>
    /// <param name="position">Position on the map.</param>
    /// <param name="width">Width of the room.</param>
    /// <param name="height">Height of the room.</param>
    /// <param name="parent">Path this room belongs to.</param>
    public Room(Point position, int width, int height, RoomPath parent) :
        this(position.X, position.Y, width, height, parent) { }

    /// <summary>
    /// Calculates position for an adjacent area (new room or probe).
    /// </summary>
    /// <param name="direction">Direction to the area from the center of the room.</param>
    /// <param name="pathLength">Length of the path between the room and area.</param>
    /// <param name="width">Width of the area.</param>
    /// <param name="height">Height of the area.</param>
    /// <returns>Position of the area.</returns>
    public Point GetAdjacentAreaPosition(Direction direction, int pathLength,
        int width, int height)
    {
        int deltaX = direction.Type switch
        {
            Direction.Types.Left => -width - pathLength,
            Direction.Types.Right => Width + pathLength,
            _ => (Width - width) / 2,
        };

        int deltaY = direction.Type switch
        {
            Direction.Types.Up => -height - pathLength,
            Direction.Types.Down => Height + pathLength,
            _ => (Height - height) / 2
        };

        // calculate position for the area
        int x = Position.X + deltaX;
        int y = Position.Y + deltaY;

        return (x, y);
    }

    /// <summary>
    /// Calculates position for an adjacent area (new room or probe).
    /// </summary>
    /// <param name="direction">Direction to the area from the center of the room.</param>
    /// <param name="pathLength">Length of the path between the room and area.</param>
    /// <param name="size">Size of both width and height of the area.</param>
    public Point GetAdjacentAreaPosition(Direction direction, int pathLength, int size) =>
        GetAdjacentAreaPosition(direction, pathLength, size, size);

    public bool TransferToPath(RoomPath newPath)
    {
        if (newPath == Path)
            return true;

        if (newPath.Count == 0)
            newPath.Add(this);
        
        else if (newPath.LastRoom.IsConnectedTo(this))
        {
            Path.Remove(this);
            newPath.Add(this);
            Path = newPath;
            return true;
        }

        return false;
    }

    public void AddEntity(Entity entity)
    {
        if (entity.Position == Point.None)
            throw new ArgumentException("Entity needs to have a valid position.");

        if (!Bounds.Contains(entity.Position))
            throw new ArgumentException("Entity position is outside the bounds of the room.");

        if (Contents.Where(e => e.Position == entity.Position).Any())
            throw new ArgumentException("Another entity already at location.");

        _contents.Add(entity);
    }

    public bool RemoveEntity(Entity entity) =>
        _contents.Remove(entity);
}