using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Furniture;

record Chest : Entity
{
    public List<Item> Items { get; } = [];

    public Chest(Point position) : base(position)
    {
        
    }
}