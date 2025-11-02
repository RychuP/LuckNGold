using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be equipped.
/// </summary>
internal interface IEquippable : ICarryable
{
    /// <summary>
    /// <see cref="IEquipment"/> slot where the <see cref="IEquippable"/> can be placed.
    /// </summary>
    BodyPart Slot { get; }
}