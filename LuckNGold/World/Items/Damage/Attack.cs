using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Defences;

namespace LuckNGold.World.Items.Damage;

record struct Attack(IPhysicalDamage PhysicalDamage,
    IElementalDamage ElementalDamage) : IAttack
{ }