using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.Generation.Monsters;

abstract record Monster : Entity
{
    public Dictionary<EquipSlot, Item> Equipment = [];
    public Monster(Point position) : base(position) { }
}