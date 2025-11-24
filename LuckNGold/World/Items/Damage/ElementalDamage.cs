using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct ElementalDamage(ElementalEffectType EffectType, IBaseDamage BaseDamage) : IElementalDamage
{
    public static readonly ElementalDamage None 
        = new(ElementalEffectType.None, Damage.BaseDamage.None);

    public static ElementalDamage Fire(int minDamage,int maxDamage) =>
        new(ElementalEffectType.Fire, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Ice(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Ice, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Lightning(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Lightning, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Poison(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Poison, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Acid(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Acid, new BaseDamage(minDamage, maxDamage));
}