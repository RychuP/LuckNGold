using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Items.Primitives;

record struct PhysicalDamage(PhysicalDamageType DamageType, IBaseDamage BaseDamage) : IPhysicalDamage
{ 
    public static readonly PhysicalDamage None = 
        new(PhysicalDamageType.None, Primitives.BaseDamage.None);

    public static PhysicalDamage Slashing(int minDamage, int maxDamage) =>
        new(PhysicalDamageType.Slashing, new BaseDamage(minDamage, maxDamage));

    public static PhysicalDamage Piercing(int minDamage, int MaxDamage) =>
        new(PhysicalDamageType.Piercing, new BaseDamage(minDamage, MaxDamage));

    public static PhysicalDamage Blunt(int minDamage, int MaxDamage) =>
        new(PhysicalDamageType.Blunt, new BaseDamage(minDamage, MaxDamage));
}