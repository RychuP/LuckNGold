using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Shields;

abstract record Shield : Item
{
    public Shield(Point position, IMaterial material) : base(position, material) { }
}