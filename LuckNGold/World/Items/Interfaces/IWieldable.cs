using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be placed in hand and used like a tool, weapon or a shield.
/// </summary>
internal interface IWieldable : ICarryable
{
    Hand Hand { get; }
}