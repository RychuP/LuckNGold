using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Decors;

abstract record Decor : Entity
{
    public Decor(Point position) : base(position)
    { }
}