using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct PotentialPhysicalDamage(PhysicalEffectType EffectType, IPotentialDamage PotentialDamage) :
    IPotentialPhysicalDamage
{
    public static readonly PotentialPhysicalDamage None =
        new(PhysicalEffectType.None, Damage.PotentialDamage.None);

    public static PotentialPhysicalDamage Slashing(int minDamage, int maxDamage) =>
        new(PhysicalEffectType.Slashing, new PotentialDamage(minDamage, maxDamage));

    public static PotentialPhysicalDamage Piercing(int minDamage, int maxDamage) =>
        new(PhysicalEffectType.Piercing, new PotentialDamage(minDamage, maxDamage));

    public static PotentialPhysicalDamage Blunt(int minDamage, int maxDamage) =>
        new(PhysicalEffectType.Blunt, new PotentialDamage(minDamage, maxDamage));

    public readonly PhysicalDamage Resolve()
    {
        int damage = PotentialDamage.Resolve();
        return new PhysicalDamage(EffectType, damage);
    }
}