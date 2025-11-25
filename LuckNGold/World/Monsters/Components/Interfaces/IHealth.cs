using LuckNGold.World.Items.Damage;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It has hit points, can die when they go below zero
/// and can suffer or benefit from transient conditions (like confused or hastened).
/// </summary>
internal interface IHealth
{
    event EventHandler<ValueChangedEventArgs<int>>? HPChanged;
    event EventHandler<IPhysicalDamage>? PhysicalDamageReceived;
    event EventHandler<IElementalDamage>? ElementalDamageReceived;

    /// <summary>
    /// Hit points remaining.
    /// </summary>
    int HP { get; }

    void ReceiveDamage(IPhysicalDamage physicalDamage);
    void ReceiveDamage(IElementalDamage elementalDamage);
}