using LuckNGold.Primitives;

namespace LuckNGold.Generation.Decors;

record Skull : Decor
{
    public HorizontalOrientation Orientation { get; init; }
    public Skull(Point position, HorizontalOrientation orientation) : base(position)
    {
        Orientation = orientation;
    }
}