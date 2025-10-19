using LuckNGold.Generation.Items;

namespace LuckNGold.Generation.Furnitures;

record Chest : Furniture
{
    public List<Item> Items { get; } = [];

    public Chest(Point position) : base(position)
    { }
}