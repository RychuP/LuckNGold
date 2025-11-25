using LuckNGold.World.Items.Damage;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons.Swords;

record ArmingSword : Sword
{
    public ArmingSword(Point position, IMetal material) : base(position, material, 
        new Dictionary<MeleeAttackType, IAttack> 
        {
            { 
                MeleeAttackType.OverheadSwing,
                new Attack(PotentialPhysicalDamage.Slashing(3, 6), PotentialElementalDamage.None)
            },
            { 
                MeleeAttackType.DiagonalSideSwing,
                new Attack(PotentialPhysicalDamage.Slashing(2, 5), PotentialElementalDamage.None)
            },
            { 
                MeleeAttackType.ForwardThrust,
                new Attack(PotentialPhysicalDamage.Piercing(1, 4), PotentialElementalDamage.None)
            }
        })
    { }
}