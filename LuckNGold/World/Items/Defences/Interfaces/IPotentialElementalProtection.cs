using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences.Interfaces;

internal interface IPotentialElementalProtection
{
    ElementalEffectType EffectType { get; }
    IPotentialProtection PotentialProtection { get; }
    ElementalProtection Resolve();
}