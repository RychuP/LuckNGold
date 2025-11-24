using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IElementalDamage
{
    ElementalEffectType EffectType { get; }
    IBaseDamage BaseDamage { get; }
}