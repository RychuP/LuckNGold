using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Damage;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons.Swords;

record ScimitarSword : Sword
{
    public ScimitarSword(Point position, IMetal material) : base(position, material,
        new Dictionary<MeleeAttackType, IAttack>
        {
            {
                MeleeAttackType.OverheadSwing,
                new Attack(PotentialPhysicalDamage.Slashing(4, 7), PotentialElementalDamage.None)
            },
            {
                MeleeAttackType.DiagonalSideSwing,
                new Attack(PotentialPhysicalDamage.Slashing(3, 6), PotentialElementalDamage.None)
            },
            {
                MeleeAttackType.ForwardThrust,
                new Attack(PotentialPhysicalDamage.Piercing(1, 3), PotentialElementalDamage.None)
            }
        })
    { }
}