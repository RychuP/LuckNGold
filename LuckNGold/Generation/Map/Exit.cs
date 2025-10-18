namespace LuckNGold.Generation.Map;

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
    public Exit? End { get; private set; } = null;

    Corridor? _corridor = null;
    /// <summary>
    /// Corridor that has this exit as one of its end points.
    /// </summary>
    public Corridor? Corridor
    {
        get => _corridor;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!value.HasExit(this))
                throw new ArgumentException("Corridor does not start or end with this exit.");
            
            _corridor = value;
            OnCorridorChange(_corridor);
        }
    }

    bool _isDouble;
    /// <summary>
    /// Whether the width of the exit is one or two tiles.
    /// </summary>
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

    /// <summary>
    /// Initializes an instance of <see cref="Exit"/> class with given parameters.
    /// </summary>
    /// <param name="position">Position of the exit.</param>
    /// <param name="room">Room the exit belongs to.</param>
    /// <exception cref="ArgumentException"></exception>
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

    void OnCorridorChange(Corridor corridor)
    {
        End = corridor.Start == this ? corridor.End : corridor.Start;
    }
}

/// <summary>
/// Exception that is thrown when no valid <see cref="Exit"/> was found 
/// where it should have been set or the exit leads to a room that
/// should not have been there.
/// </summary>
public class MissingOrNotValidExitException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingOrNotValidExitException"/>.
    /// </summary>
    public MissingOrNotValidExitException() { }
}