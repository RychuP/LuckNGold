namespace LuckNGold.Generation.Map;

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
    /// Corner positions of the room.
    /// </summary>
    public Point[] CornerPositions { get; }

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
}
