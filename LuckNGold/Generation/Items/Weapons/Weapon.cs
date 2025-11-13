using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Items.Weapons;

abstract record Weapon : Item
{
    public Material Material { get; init; }

    public Weapon(Point position, Material material) : base(position)
    {
        Material = material;
    }
}