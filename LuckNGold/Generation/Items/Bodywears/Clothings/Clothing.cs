using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Bodywears.Clothings;

abstract record Clothing : Bodywear
{
    public Clothing(Point position, IFabric material) : base(position, material)
    {

    }
}