using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Furnitures;

abstract record Furniture : Entity
{
    public Furniture(Point position) : base(position)
    { }
}