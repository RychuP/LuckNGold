using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Footwears.Boots;

abstract record Boot : Footwear
{
    public Boot(Point position, IMaterial material) : base(position, material) { }
}