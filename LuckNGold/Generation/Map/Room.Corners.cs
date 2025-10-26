using GoRogue.Random;

namespace LuckNGold.Generation.Map;

// Properties and methods relating to the room corners and their neighbours.
partial class Room
{
    /// <summary>
    /// Corner positions of the room in order:
    /// top left, top right, bottom left and bottom right.
    /// </summary>
    public Point[] CornerPositions { get; }


    Point[] GetCornerPositions()
    {
        return [
            Position,
            (Area.MaxExtentX, Area.MinExtentY),
            (Area.MinExtentX, Area.MaxExtentY),
            (Area.MaxExtentX, Area.MaxExtentY)
        ];
    }

    public Point GetRandomCorner()
    {
        int cornerIndex = GlobalRandom.DefaultRNG.NextInt(4);
        return CornerPositions[cornerIndex];
    }

    /// <summary>
    /// Gets 3 corner neighbour positions. First one is horizontal,
    /// second vertical and third diagonal.
    /// </summary>
    /// <param name="cornerIndex">Index of the corner in <see cref="CornerPositions"/>.</param>
    /// <returns>Array of 3 corner neighbours.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Point[] GetCornerNeighbours(int cornerIndex)
    {
        if (cornerIndex < 0 || cornerIndex >= CornerPositions.Length)
            throw new ArgumentOutOfRangeException(nameof(cornerIndex));

        return GetCornerNeighbours(CornerPositions[cornerIndex]);
    }


    /// <summary>
    /// Gets 3 corner neighbour positions. First one is horizontal,
    /// second vertical and third diagonal.
    /// </summary>
    /// <param name="cornerPosition">Corner position of the room.</param>
    /// <returns>Array of 3 corner neighbours.</returns>
    /// <exception cref="ArgumentException"></exception>
    public Point[] GetCornerNeighbours(Point cornerPosition)
    {
        int cornerIndex = Array.IndexOf(CornerPositions, cornerPosition);
        if (cornerIndex < 0)
            throw new ArgumentException("Position is not a corner of this room.", 
                nameof(cornerPosition));

        Direction delta1 = cornerIndex switch
        {
            0 or 2 => Direction.Right,
            _ => Direction.Left,
        };

        Direction delta2 = cornerIndex switch
        {
            0 or 1 =>  Direction.Down,
            _ => Direction.Up,
        };

        Direction delta3 = cornerIndex switch
        {
            0 => Direction.DownRight,
            1 => Direction.DownLeft,
            2 => Direction.UpRight,
            _ => Direction.UpLeft
        };

        return [cornerPosition + delta1, cornerPosition + delta2, cornerPosition + delta3];
    }
}