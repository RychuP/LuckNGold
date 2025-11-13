using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.Generation.Items.Weapons;

abstract record MeleeWeapon : Weapon
{
    public Dictionary<MeleeAttackType, IAttackDamage> Attacks { get; init; }

    public MeleeWeapon(Point position, Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) : base(position, material)
    {
        Attacks = attacks;
    }
}