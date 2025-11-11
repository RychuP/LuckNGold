using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

internal interface IPhysicalDamage
{
    PhysicalDamageType DamageType { get; }
    IBaseDamage BaseDamage { get; }
}