using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct ElementalDamage(ElementalDamageType DamageType, IBaseDamage BaseDamage) : IElementalDamage
{
    public static readonly ElementalDamage None 
        = new(ElementalDamageType.None, Damage.BaseDamage.None);

    public static ElementalDamage Fire(int minDamage,int maxDamage) =>
        new(ElementalDamageType.Fire, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Ice(int minDamage, int maxDamage) =>
        new(ElementalDamageType.Ice, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Lightning(int minDamage, int maxDamage) =>
        new(ElementalDamageType.Lightning, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Poison(int minDamage, int maxDamage) =>
        new(ElementalDamageType.Poison, new BaseDamage(minDamage, maxDamage));

    public static ElementalDamage Acid(int minDamage, int maxDamage) =>
        new(ElementalDamageType.Acid, new BaseDamage(minDamage, maxDamage));
}