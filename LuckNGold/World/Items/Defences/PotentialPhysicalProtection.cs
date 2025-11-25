using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct PotentialPhysicalProtection(PhysicalEffectType EffectType, 
    IPotentialProtection PotentialProtection) : IPotentialPhysicalProtection
{
    public static readonly PotentialPhysicalProtection None =
        new(PhysicalEffectType.None, Defences.PotentialProtection.None);

    public static PotentialPhysicalProtection Slashing(int minProtection, int maxProtection) =>
    new(PhysicalEffectType.Slashing, new PotentialProtection(minProtection, maxProtection));

    public static PotentialPhysicalProtection Piercing(int minProtection, int maxProtection) =>
        new(PhysicalEffectType.Piercing, new PotentialProtection(minProtection, maxProtection));

    public static PotentialPhysicalProtection Blunt(int minProtection, int maxProtection) =>
        new(PhysicalEffectType.Blunt, new PotentialProtection(minProtection, maxProtection));

    public readonly PhysicalProtection Resolve()
    {
        int protection = PotentialProtection.Resolve();
        return new PhysicalProtection(EffectType, protection);
    }
}