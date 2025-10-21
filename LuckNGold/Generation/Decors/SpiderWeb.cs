using LuckNGold.Primitives;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Decors;

record SpiderWeb : Decor
{
    public HorizontalOrientation Orientation { get; init; }
    public Size Size { get; init; }
    public SpiderWeb(Point position, Size size, HorizontalOrientation orientation) 
        : base(position)
    {
        Orientation = orientation;
        Size = size;
    }
}