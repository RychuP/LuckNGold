using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Helmets;

abstract record Helmet : Item
{
    public Helmet(Point position, IMaterial material) : base(position, material) { }
}