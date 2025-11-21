using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Footwears.Shoes;

record PeasantShoes : Shoe
{
    public PeasantShoes(Point position, ILeather material) : base(position, material) { }
}