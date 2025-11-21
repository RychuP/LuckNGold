using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Collectables;

abstract record Collectable : Item
{
    public Collectable(Point position, IMaterial material) : base(position, material) { }
}