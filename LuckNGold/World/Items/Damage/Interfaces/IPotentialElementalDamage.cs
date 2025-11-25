using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IPotentialElementalDamage
{
    ElementalEffectType EffectType { get; }
    IPotentialDamage PotentialDamage { get; }
    ElementalDamage Resolve();
}