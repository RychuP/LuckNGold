using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Monsters;

abstract record Monster : Entity
{
    public Monster(Point position) : base(position) { }
}