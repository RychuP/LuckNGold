using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Items.Primitives;

record struct AttackDamage(IPhysicalDamage PhysicalDamage,
    IElementalDamage ElementalDamage) : IAttackDamage
{ }