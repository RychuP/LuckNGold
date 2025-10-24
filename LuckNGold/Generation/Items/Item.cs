using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Items;

abstract record Item : Entity
{
    public Item(Point position) : base(position)
    { }
}