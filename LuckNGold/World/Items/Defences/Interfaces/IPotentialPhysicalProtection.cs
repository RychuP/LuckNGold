using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences.Interfaces;

internal interface IPotentialPhysicalProtection
{
    PhysicalEffectType EffectType { get; }
    IPotentialProtection PotentialProtection { get; }
    PhysicalProtection Resolve();
}