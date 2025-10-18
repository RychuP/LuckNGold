using GoRogue.Random;
using LuckNGold.World.Items.Enums;
using SadRogue.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.Generation.Map;

class Room
{
    /// <summary>
    /// Min odd wall size.
    /// </summary>
    /// <remarks>Cannot be set to less than 3.</remarks>
    public const int MinOddSize = 3;

    /// <summary>
    /// Max odd wall size.
    /// </summary>
    public const int MaxOddSize = 9;

    public const int MinEvenSize = MinOddSize + 1;

    public const int MaxEvenSize = MaxOddSize + 1;

    /// <summary>
    /// Minimum ratio of shorter length to longer length. 
    /// </summary>
    public const double MinSizeRatio = 0.65d;

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
    /// Number of rooms between this room and the exit room of the section.
    /// </summary>
    public int DistanceToSectionExit { get; set; }

    /// <summary>
    /// List of exits and dead ends.
    /// </summary>
    public List<IWallConnection> Connections { get; } = new(4);

    /// <summary>
    /// <see cref="RoomPath"/> that this room belongs to.
    /// </summary>
    public RoomPath Path { get; private set; }

    RoomType _type;
    public RoomType Type
    {
        get => _type;
        set
        {
            if (_type != RoomType.None)
                throw new InvalidOperationException("Room type doesn't change once set.");

            if (value == RoomType.DungeonEntrance
                && (Path.Name != "MainPath" || Path.FirstRoom != this))
                throw new InvalidOperationException("This room is not the dungeon entrance.");
            else if (value == RoomType.DungeonExit &&
                (Path.Name != "MainPath" || Path.LastRoom != this))
                throw new InvalidOperationException("This room is not the dungeon exit.");


            _type = value;
        }
    }

    /// <summary>
    /// Wall directions.
    /// </summary>
    public static Direction[] Directions =>
        AdjacencyRule.Cardinals.DirectionsOfNeighborsCache;

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
    /// Checks if the room has walls that can accept new exits.
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
    /// Tries to get <see cref="Exit"/> to the given <see cref="Room"/>.
    /// </summary>
    /// <param name="room">Room to which exit needs to be found.</param>
    /// <param name="exit">Exit to the room if found.</param>
    /// <returns>True if exit was found, false otherwise.</returns>
    public bool TryGetExit(Room room, [NotNullWhen(true)] out Exit? exit)
    {
        exit = Exits.Where(e => e.End!.Room == room).First();
        return exit is not null;
    }

    /// <summary>
    /// Gets the direction to a random wall of the room available to add a connection.
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

    /// <summary>
    /// Gets the point on the wall in the given direction where a connection can be placed.
    /// </summary>
    /// <param name="direction">Direction from the center of the room
    /// to the wall to be checked.</param>
    /// <returns>Position for the connection.</returns>
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

    /// <summary>
    /// Checks if this room is the start room of side paths.
    /// </summary>
    /// <returns>True if the room is a start room of another path, false otherwise.</returns>
    public bool IsStartRoom() =>
        SidePathExits.Any();

    /// <summary>
    /// Exits leading to other paths
    /// not including the parent path of the room's path.
    /// </summary>
    public IEnumerable<Exit> SidePathExits => Exits.
        Where(e => e.End!.Room.Path != Path && e.End!.Room.Path != Path.Parent);

    /// <summary>
    /// Side paths connected directly to the room.
    /// </summary>
    public IEnumerable<RoomPath> SidePaths => SidePathExits
        .Select(e => e.End!.Room.Path);

    /// <summary>
    /// Exits leading out of the room.
    /// </summary>
    public IEnumerable<Exit> Exits => Connections
        .Where(c => c is Exit)
        .Cast<Exit>();
}