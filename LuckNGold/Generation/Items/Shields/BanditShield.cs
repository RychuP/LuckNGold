using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Shields;

record BanditShield : Shield
{
    public BanditShield(Point position, IWood material) : base(position, material)
    {

    }
}