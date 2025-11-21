using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Footwears;

abstract record Footwear : Item
{
    public Footwear(Point position, IMaterial material) : base(position, material) { }
}