using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct PotentialElementalDamage(ElementalEffectType EffectType, IPotentialDamage PotentialDamage) : 
    IPotentialElementalDamage
{
    public static readonly PotentialElementalDamage None 
        = new(ElementalEffectType.None, Damage.PotentialDamage.None);

    public static PotentialElementalDamage Fire(int minDamage, int maxDamage) =>
    new(ElementalEffectType.Fire, new PotentialDamage(minDamage, maxDamage));

    public static PotentialElementalDamage Ice(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Ice, new PotentialDamage(minDamage, maxDamage));

    public static PotentialElementalDamage Lightning(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Lightning, new PotentialDamage(minDamage, maxDamage));

    public static PotentialElementalDamage Poison(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Poison, new PotentialDamage(minDamage, maxDamage));

    public static PotentialElementalDamage Acid(int minDamage, int maxDamage) =>
        new(ElementalEffectType.Acid, new PotentialDamage(minDamage, maxDamage));

    public readonly ElementalDamage Resolve()
    {
        var damage = PotentialDamage.Resolve();
        return new ElementalDamage(EffectType, damage);
    }
}