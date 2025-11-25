using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IPotentialPhysicalDamage
{
    PhysicalEffectType EffectType { get; }
    IPotentialDamage PotentialDamage { get; }
    PhysicalDamage Resolve();
}