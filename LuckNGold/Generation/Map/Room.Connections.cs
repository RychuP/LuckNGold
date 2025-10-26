using GoRogue.Random;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.Generation.Map;

// Properties and methods relating to creation and querying of room connections.
partial class Room
{
    /// <summary>
    /// List of exits and dead ends.
    /// </summary>
    public List<IWallConnection> Connections { get; } = new(4);

    /// <summary>
    /// Directions to the walls from room center.
    /// </summary>
    public static Direction[] WallDirections =>
        AdjacencyRule.Cardinals.DirectionsOfNeighborsCache;

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
    /// Checks if the room has walls that can accept new exits.
    /// </summary>
    public bool HasAvailableConnections() =>
        Connections.Count < 4;

    public bool IsConnectedTo(Room room)
    {
        foreach (var exit in Exits)
            if (exit.End!.Room == room) return true;
        return false;
    }

    /// <summary>
    /// Tries to get <see cref="Exit"/> in the given direction.
    /// </summary>
    /// <param name="direction"><see cref="Direction"/> to the <see cref="Exit"/>
    /// from the center of the <see cref="Room"/>.</param>
    /// <param name="exit"><see cref="Exit"/> if found.</param>
    /// <returns>True if <see cref="Exit"/> exists, or false otherwise.</returns>
    public bool TryGetExit(Direction direction, [NotNullWhen(true)] out Exit? exit)
    {
        exit = Exits.Where(c => c.Direction == direction).FirstOrDefault();
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
        exit = Exits.Where(e => e.End!.Room == room).FirstOrDefault();
        return exit is not null;
    }

    public bool TryGetConnection(Direction direction,
        [NotNullWhen(true)] out IWallConnection? connection)
    {
        connection = Connections.Where(c => c.Direction == direction).FirstOrDefault();
        return connection is not null;
    }

    /// <summary>
    /// Gets the direction to a random wall of the room available to add a connection.
    /// </summary>
    public Direction GetRandomAvailableConnection()
    {
        var availableDirections = WallDirections.Except(Connections.Select(c => c.Direction));
        List<Direction> directionPool = [.. availableDirections];
        if (directionPool.Count == 0)
            throw new InvalidOperationException("No available connections left to choose from.");
        var index = GlobalRandom.DefaultRNG.NextInt(directionPool.Count);
        return directionPool[index];
    }

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

    /// <summary>
    /// Checks if this room is the start room of side paths.
    /// </summary>
    /// <returns>True if the room is a start room of another path, false otherwise.</returns>
    public bool IsPathStartRoom() =>
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