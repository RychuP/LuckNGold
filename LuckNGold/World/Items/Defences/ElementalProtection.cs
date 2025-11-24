using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct ElementalProtection(ElementalEffectType EffectType, IBaseProtection BaseProtection) : 
    IElementalProtection
{
    public static ElementalProtection Fire(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Fire, new BaseProtection(minProtection, maxProtection));

    public static ElementalProtection Ice(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Ice, new BaseProtection(minProtection, maxProtection));

    public static ElementalProtection Lightning(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Lightning, new BaseProtection(minProtection, maxProtection));

    public static ElementalProtection Poison(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Poison, new BaseProtection(minProtection, maxProtection));

    public static ElementalProtection Acid(int minProtection, int maxProtection) =>
        new(ElementalEffectType.Acid, new BaseProtection(minProtection, maxProtection));
}