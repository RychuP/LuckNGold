using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct PhysicalProtection(PhysicalEffectType EffectType, IBaseProtection BaseProtection) : 
    IPhysicalProtection
{
    public static PhysicalProtection Slashing(int minProtection, int maxProtection) =>
        new(PhysicalEffectType.Slashing, new BaseProtection(minProtection, maxProtection));

    public static PhysicalProtection Piercing(int minProtection, int maxProtection) =>
        new(PhysicalEffectType.Piercing, new BaseProtection(minProtection, maxProtection));

    public static PhysicalProtection Blunt(int minProtection, int maxProtection) =>
        new(PhysicalEffectType.Blunt, new BaseProtection(minProtection, maxProtection));
}