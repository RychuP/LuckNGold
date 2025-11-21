using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Footwears.Shoes;

abstract record Shoe : Footwear
{
    public Shoe(Point position, IMaterial material) : base(position, material) { }
}