using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons;

abstract record Weapon : Item
{
    public Weapon(Point position, IMaterial material) : base(position, material)
    {
        
    }
}