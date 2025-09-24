using GoRogue.Random;

namespace LuckNGold;

static class DirectionExtensions
{
    static readonly Direction[] s_verticalDirections = [Direction.Up, Direction.Down];
    static readonly Direction[] s_horizontalDirections = [Direction.Left, Direction.Right];

    public static Direction[] GetPerpendicularDirections(this Direction direction)
    {
        return direction.Type switch
        {
            Direction.Types.Up => s_horizontalDirections,
            Direction.Types.Down => s_horizontalDirections,
            _ => s_verticalDirections
        };
    }

    public static bool IsVertical(this Direction direction) =>
        s_verticalDirections.Contains(direction);

    public static bool IsHorizontal(this Direction direction) =>
        s_horizontalDirections.Contains(direction);

    public static Direction GetOpposite(this Direction direction)
    {
        if (direction.IsVertical())
            return direction == Direction.Up ? Direction.Down : Direction.Up;
        else if (direction.IsHorizontal())
            return direction == Direction.Left ? Direction.Right : Direction.Left;
        else
            throw new ArgumentException($"Direction {nameof(direction)} is not cardinal.");
    }
}

static class IntExtensions
{
    public static bool IsEven(this int value) =>
        value % 2 == 0;

    public static bool IsOdd(this int value) =>
        !value.IsEven();
}

static class ColorExtensions
{
    
}