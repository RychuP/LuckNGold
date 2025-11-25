using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct PhysicalProtection(PhysicalEffectType EffectType, int Amount) : IPhysicalProtection
{
    public static readonly PhysicalProtection None = new(PhysicalEffectType.None, 0);
}