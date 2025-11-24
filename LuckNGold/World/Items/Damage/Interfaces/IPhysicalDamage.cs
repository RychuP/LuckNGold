using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IPhysicalDamage
{
    PhysicalEffectType EffectType { get; }
    IBaseDamage BaseDamage { get; }
}