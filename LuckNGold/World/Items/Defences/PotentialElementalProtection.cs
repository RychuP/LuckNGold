using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct PotentialElementalProtection(ElementalEffectType EffectType,
    IPotentialProtection PotentialProtection) : IPotentialElementalProtection
{
    public static readonly PotentialElementalProtection None =
        new(ElementalEffectType.None, Defences.PotentialProtection.None);

    public static PotentialElementalProtection Fire(int minProtection, int maxProtection) =>
    new(ElementalEffectType.Fire, new PotentialProtection(minProtection, maxProtection));

    public static PotentialElementalProtection Ice(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Ice, new PotentialProtection(minProtection, maxProtection));

    public static PotentialElementalProtection Lightning(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Lightning, new PotentialProtection(minProtection, maxProtection));

    public static PotentialElementalProtection Poison(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Poison, new PotentialProtection(minProtection, maxProtection));

    public static PotentialElementalProtection Acid(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Acid, new PotentialProtection(minProtection, maxProtection));

    public readonly ElementalProtection Resolve()
    {
        int protection = PotentialProtection.Resolve();
        return new ElementalProtection(EffectType, protection);
    }
}