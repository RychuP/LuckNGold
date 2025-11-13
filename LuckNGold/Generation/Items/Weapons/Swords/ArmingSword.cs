using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Items.Primitives;

namespace LuckNGold.Generation.Items.Weapons.Swords;

record ArmingSword : Sword
{
    public ArmingSword(Point position, Material material) : base(position, material, 
        new Dictionary<MeleeAttackType, IAttackDamage> 
        {
            { 
                MeleeAttackType.OverheadSwing,
                new AttackDamage(PhysicalDamage.Slashing(3, 6), ElementalDamage.None)
            },
            { 
                MeleeAttackType.DiagonalSideSwing,
                new AttackDamage(PhysicalDamage.Slashing(2, 5), ElementalDamage.None)
            },
            { 
                MeleeAttackType.ForwardThrust,
                new AttackDamage(PhysicalDamage.Piercing(1, 4), ElementalDamage.None)
            }
        })
    { }
}