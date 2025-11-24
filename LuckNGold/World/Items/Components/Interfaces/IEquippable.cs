using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;

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