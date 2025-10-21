using LuckNGold.Primitives;

namespace LuckNGold.Generation.Decors;

record Fountain : Decor
{
    public VerticalOrientation Orientation { get; init; }
    public bool IsBlue { get; init; }

    public Fountain(Point position, VerticalOrientation orientation, bool isBlue) :
        base(position)
    {
        Orientation = orientation;
        IsBlue = isBlue;
    }
}