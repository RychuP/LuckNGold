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
                new Attack(PhysicalDamage.Slashing(3, 6), ElementalDamage.None)
            },
            { 
                MeleeAttackType.DiagonalSideSwing,
                new Attack(PhysicalDamage.Slashing(2, 5), ElementalDamage.None)
            },
            { 
                MeleeAttackType.ForwardThrust,
                new Attack(PhysicalDamage.Piercing(1, 4), ElementalDamage.None)
            }
        })
    { }
}