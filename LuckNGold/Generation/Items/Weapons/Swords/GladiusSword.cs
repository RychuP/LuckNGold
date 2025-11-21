using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Damage;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons.Swords;

record GladiusSword : Sword
{
    public GladiusSword(Point position, IMetal material) : base(position, material,
        new Dictionary<MeleeAttackType, IAttackDamage>
        {
            {
                MeleeAttackType.OverheadSwing,
                new AttackDamage(PhysicalDamage.Slashing(2, 5), ElementalDamage.None)
            },
            {
                MeleeAttackType.DiagonalSideSwing,
                new AttackDamage(PhysicalDamage.Slashing(2, 5), ElementalDamage.None)
            },
            {
                MeleeAttackType.ForwardThrust,
                new AttackDamage(PhysicalDamage.Piercing(3, 6), ElementalDamage.None)
            }
        })
    { }
}