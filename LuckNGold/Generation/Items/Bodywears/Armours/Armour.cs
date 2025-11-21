using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Bodywears.Armours;

abstract record Armour : Bodywear
{
    public Armour(Point position, IMaterial material) : base(position, material) { }
}