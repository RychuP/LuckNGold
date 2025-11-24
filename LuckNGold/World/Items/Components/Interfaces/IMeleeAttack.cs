using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Components.Interfaces;

/// <summary>
/// It can perform various melee attacks.
/// </summary>
internal interface IMeleeAttack
{
    /// <summary>
    /// Dictionary of available <see cref="MeleeAttackType"/>s with their <see cref="IAttack"/>s.
    /// </summary>
    IReadOnlyDictionary<MeleeAttackType, IAttack> Attacks { get; }
}