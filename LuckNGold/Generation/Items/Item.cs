using LuckNGold.Generation.Map;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items;

abstract record Item : Entity
{
    public IMaterial Material { get; }

    public Item(Point position, IMaterial material) : base(position)
    {
        Material = material;
    }
}