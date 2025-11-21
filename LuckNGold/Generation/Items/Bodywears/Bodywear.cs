using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Bodywears;

abstract record Bodywear : Item
{
    public Bodywear(Point position, IMaterial material) : base(position, material) { }
}