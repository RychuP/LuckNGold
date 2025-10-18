using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Items;

record Key : Item
{
    public Gemstone Material { get; init; }

    public Key(Point position, Gemstone material) : base(position)
    {
        Material = material;
    }
}