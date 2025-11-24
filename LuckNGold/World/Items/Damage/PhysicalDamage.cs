using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct PhysicalDamage(PhysicalEffectType EffectType, IBaseDamage BaseDamage) : IPhysicalDamage
{ 
    public static readonly PhysicalDamage None = 
        new(PhysicalEffectType.None, Damage.BaseDamage.None);

    public static PhysicalDamage Slashing(int minDamage, int maxDamage) =>
        new(PhysicalEffectType.Slashing, new BaseDamage(minDamage, maxDamage));

    public static PhysicalDamage Piercing(int minDamage, int MaxDamage) =>
        new(PhysicalEffectType.Piercing, new BaseDamage(minDamage, MaxDamage));

    public static PhysicalDamage Blunt(int minDamage, int MaxDamage) =>
        new(PhysicalEffectType.Blunt, new BaseDamage(minDamage, MaxDamage));
}