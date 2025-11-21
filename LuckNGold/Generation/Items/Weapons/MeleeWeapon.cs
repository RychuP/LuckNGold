using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons;

abstract record MeleeWeapon : Weapon
{
    public Dictionary<MeleeAttackType, IAttackDamage> Attacks { get; init; }

    public MeleeWeapon(Point position, IMaterial material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) : base(position, material)
    {
        Attacks = attacks;
    }
}