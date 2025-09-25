using GoRogue.Random;
using SadRogue.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.Generation;

class Room
{
    // min odd wall size (minimum 3)
    public const int MinOddSize = 3;

    // max odd wall size
    public const int MaxOddSize = 9;

    public const int MinEvenSize = MinOddSize + 1;

    public const int MaxEvenSize = MaxOddSize + 1;

    /// <summary>
    /// Minimum ratio of shorter length to longer length. 
    /// </summary>
    public const double MinSizeRatio = 0.65d;
    
    // inner area of the room
    public Rectangle Area { get; }

    // area with added wall space around needed to display the walls properly
    public Rectangle Bounds { get; }

    // width of the room
    public int Width => Area.Width;

    // height of the room
    public int Height => Area.Height;

    // position of the room
    public Point Position => Area.Position;

    // list of exits and dead ends
    public List<IWallConnection> Connections { get; } = new(4);

    public RoomPath Path { get; private set; }

    public Room(int x, int y, int width, int height, RoomPath parent)
    {
        Area = new Rectangle(x, y, width, height);
        Bounds = Area.Expand(1, 1);
        Path = parent;
    }

    public Room(Point position, int width, int height, RoomPath parent) :
        this(position.X, position.Y, width, height, parent) { }

    /// <summary>
    /// Creates a new <see cref="Exit"/> in the given direction.
    /// </summary>
    /// <param name="direction">Direction of the wall where the exit is located.</param>
    /// <returns>Instance of the new <see cref="Exit"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if a connection already exists.</exception>
    public Exit AddExit(Direction direction)
    {
        if (Connections.Any(c => c.Direction == direction))
            throw new ArgumentException($"Connection to the {nameof(direction)} already exists.");

        var position = GetConnectionPoint(direction);
        var exit = new Exit(position, this);
        Connections.Add(exit);
        return exit;
    }

    /// <summary>
    /// Checks if the room has walls that can accept new exits
    /// </summary>
    public bool HasAvailableConnections() =>
        Connections.Count < 4;

    public bool IsConnectedTo(Room room)
    {
        foreach (var connection in Connections)
        {
            if (connection is Exit exit && exit.End?.Room == room) return true;
        }
        return false;
    }


    public bool TryGetExit(Direction direction, [NotNullWhen(true)] out Exit? exit)
    {
        var connection = Connections.Find(c => c.Direction == direction);
        exit = connection is Exit e ? e : null;
        return exit is not null;
    }

    /// <summary>
    /// Returns the direction to a random wall available to add a connection.
    /// </summary>
    public Direction GetRandomAvailableConnection()
    {
        var availableDirections = Directions.Except(Connections.Select(c => c.Direction));
        List<Direction> directionPool = [.. availableDirections];
        if (directionPool.Count == 0)
            throw new InvalidOperationException("No available connections left to choose from.");
        var index = GlobalRandom.DefaultRNG.NextInt(directionPool.Count);
        return directionPool[index];
    }

    /// <summary>
    /// Adds a new <see cref="DeadEnd"/> to <see cref="Connections"/> in the given direction.
    /// </summary>
    /// <param name="direction"></param>
    public void AddDeadEnd(Direction direction)
    {
        if (Connections.Any(c => c.Direction == direction))
            throw new ArgumentException($"Connection to the {nameof(direction)} already exists.");

        Connections.Add(new DeadEnd(direction));
    }

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

    /// <summary>
    /// Returns the size of the wall in the given direction from the center.
    /// </summary>
    public int GetWallSize(Direction direction) =>
        direction.IsHorizontal() ? Height : Width;

    // returns the point on the wall in the given direction where an exit can be placed
    public Point GetConnectionPoint(Direction direction)
    {
        int horizontalWallMiddleX = Position.X + (Width.IsOdd() ? Width / 2 : Width / 2 - 1);
        int verticalWallMiddleY = Position.Y + Height / 2;

        return direction.Type switch
        {
            Direction.Types.Up => (horizontalWallMiddleX, Position.Y - 1),
            Direction.Types.Down => (horizontalWallMiddleX, Area.MaxExtentY + 1),
            Direction.Types.Left => (Position.X - 1, verticalWallMiddleY),
            _ => (Area.MaxExtentX + 1, verticalWallMiddleY)
        };
    }

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

    #region Static Members
    // wall directions
    public static Direction[] Directions =>
        AdjacencyRule.Cardinals.DirectionsOfNeighborsCache;

    public static int GetRandomOddSize(int min = MinOddSize, int max = MaxOddSize)
    {
        int size;
        do { size = GlobalRandom.DefaultRNG.NextInt(min, max + 1); }
        while (size.IsEven());
        return size;
    }

    public static int GetRandomEvenSize(int min = MinEvenSize, int max = MaxEvenSize)
    {
        int size;
        do { size = GlobalRandom.DefaultRNG.NextInt(min, max + 1); }
        while (size.IsOdd());
        return size;
    }

    public static int GetRandomSize(int min = MinOddSize, int max = MaxEvenSize) =>
        GlobalRandom.DefaultRNG.NextInt(min, max + 1);
    #endregion
}