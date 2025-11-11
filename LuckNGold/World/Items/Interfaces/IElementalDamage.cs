using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

internal interface IElementalDamage
{
    ElementalDamageType DamageType { get; }
    IBaseDamage BaseDamage { get; }
}