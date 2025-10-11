namespace LuckNGold.Generation;

internal class Exit : IWallConnection
{
    /// <summary>
    /// Position of the exit.
    /// </summary>
    public Point Position { get; }

    /// <summary>
    /// Room where the exit is located.
    /// </summary>
    public Room Room { get; }

    /// <summary>
    /// Direction from the room center to the wall where the exit is located.
    /// </summary>
    public Direction Direction { get; }

    /// <summary>
    /// Destination <see cref="Exit"/> where this exit leads to.
    /// </summary>
    public Exit? End
    {
        get
        {
            if (Corridor is not null)
            {
                return Corridor.Start == this ? Corridor.End : Corridor.Start;
            }
            return null;
        }
    }

    Corridor? _corridor = null;
    public Corridor? Corridor
    {
        get => _corridor;
        set
        {
            if (value is not null && !value.HasExit(this))
                throw new ArgumentException("Given corridor does not start or end with this exit.");
            
            _corridor = value;
        }
    }

    /// <summary>
    /// Whether the width of the exit is double (can accommodate a door) or single tile
    /// </summary>
    bool _isDouble;
    public bool IsDouble
    {
        get => _isDouble;
        init
        {
            if (value == true && Direction.IsHorizontal())
                throw new ArgumentException("Exits on vertical walls cannot " +
                    "be marked as double.");

            _isDouble = value;
        }
    }

    public Exit(Point position, Room room)
    {
        if (!room.Area.Expand(1, 1).PerimeterPositions().Contains(position))
            throw new ArgumentException("Position of the exit must be on the perimeter " +
                "of the room " + nameof(room));

        Position = position;
        Room = room;
        Direction = Direction.GetCardinalDirection(room.Area.Center, position);

        // check the size of the corridor
        int wallSize = room.GetWallSize(Direction);
        IsDouble = wallSize.IsEven();
    }
}