using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

namespace LuckNGold.World.Items.Components.Interfaces;

/// <summary>
/// It can be equipped.
/// </summary>
internal interface IEquippable : ICarryable
{
    /// <summary>
    /// Slot in <see cref="IEquipment"/> where the <see cref="IEquippable"/> can be placed.
    /// </summary>
    EquipSlot Slot { get; }
}