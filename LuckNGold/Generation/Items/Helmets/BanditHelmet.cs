using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Helmets;

record BanditHelmet : Helmet
{
    public BanditHelmet(Point position, IMetal material) : base(position, material)
    {

    }
}