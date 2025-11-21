using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Tools;

abstract record Tool : Item
{
    public Tool(Point position, IMaterial material) : base(position, material) { }
}