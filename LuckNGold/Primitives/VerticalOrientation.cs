namespace LuckNGold.Primitives;

readonly struct VerticalOrientation : IEquatable<VerticalOrientation>
{
    public readonly Types Type;
    public enum Types
    {
        Top,
        Bottom
    }

    private VerticalOrientation(Types type) => Type = type;

    public static readonly VerticalOrientation Top = new(Types.Top);
    public static readonly VerticalOrientation Bottom = new(Types.Bottom);

    public override string ToString() => Type.ToString();
    public bool Equals(VerticalOrientation other) => Type == other.Type;
    public override bool Equals(object? obj) => obj is VerticalOrientation other && Equals(other);
    public override int GetHashCode() => Type.GetHashCode();
    public static bool operator ==(VerticalOrientation left, VerticalOrientation right)
        => left.Equals(right);
    public static bool operator !=(VerticalOrientation left, VerticalOrientation right)
        => !left.Equals(right);

    public static implicit operator Direction(VerticalOrientation verticalOrientation) =>
        verticalOrientation.Type switch
        {
            Types.Top => Direction.Up,
            _ => Direction.Down,
        };

    public static implicit operator VerticalOrientation(Direction direction) =>
        direction.Type switch
        {
            Direction.Types.Up => Top,
            _ => Bottom,
        };
}