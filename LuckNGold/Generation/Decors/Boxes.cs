using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Decors;

record Boxes : Decor
{
    public Size Size { get; init; }

    public Boxes(Point position, Size size) : base(position)
    {
        Size = size;
    }
}