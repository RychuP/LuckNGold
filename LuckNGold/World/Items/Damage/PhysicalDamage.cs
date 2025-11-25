using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct PhysicalDamage(PhysicalEffectType EffectType, int Amount) : IPhysicalDamage
{ 
    public static readonly PhysicalDamage None = new(PhysicalEffectType.None, 0);
}