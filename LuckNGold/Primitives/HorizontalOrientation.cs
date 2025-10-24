namespace LuckNGold.Primitives;

readonly struct HorizontalOrientation : IEquatable<HorizontalOrientation>
{
    public readonly Types Type;
    public enum Types
    {
        Left,
        Right
    }

    private HorizontalOrientation(Types type) => Type = type;

    public static readonly HorizontalOrientation Left = new(Types.Left);
    public static readonly HorizontalOrientation Right = new(Types.Right);

    public override string ToString() => Type.ToString();
    public bool Equals(HorizontalOrientation other) => Type == other.Type;
    public override bool Equals(object? obj) => obj is HorizontalOrientation other && Equals(other);
    public override int GetHashCode() => Type.GetHashCode();
    public static bool operator ==(HorizontalOrientation left, HorizontalOrientation right)
        => left.Equals(right);
    public static bool operator !=(HorizontalOrientation left, HorizontalOrientation right)
        => !left.Equals(right);

    public static implicit operator Direction(HorizontalOrientation horizontalOrientation) =>
        horizontalOrientation.Type switch
        {
            Types.Right => Direction.Right,
            _ => Direction.Left
        };

    public static implicit operator HorizontalOrientation(Direction direction) =>
        direction.Type switch
        {
            Direction.Types.Right => Right,
            _ => Left,
        };
}