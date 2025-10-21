using LuckNGold.Primitives;

namespace LuckNGold.Generation.Decors;

record SideTorch : Decor
{
    public HorizontalOrientation Orientation { get; init; }

    public SideTorch(Point position, HorizontalOrientation orientation) : base(position)
    {
        Orientation = orientation;
    }
}