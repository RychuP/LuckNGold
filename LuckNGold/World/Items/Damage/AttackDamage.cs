using LuckNGold.World.Items.Damage.Interfaces;

namespace LuckNGold.World.Items.Damage;

record struct AttackDamage(IPhysicalDamage PhysicalDamage,
    IElementalDamage ElementalDamage) : IAttackDamage
{ }