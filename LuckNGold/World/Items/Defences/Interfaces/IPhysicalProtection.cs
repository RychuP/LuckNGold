using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences.Interfaces;

internal interface IPhysicalProtection
{
    PhysicalEffectType EffectType { get; }
    IBaseProtection BaseProtection { get; }
}