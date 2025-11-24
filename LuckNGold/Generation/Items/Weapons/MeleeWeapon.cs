using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons;

abstract record MeleeWeapon : Weapon
{
    public Dictionary<MeleeAttackType, IAttack> Attacks { get; init; }

    public MeleeWeapon(Point position, IMaterial material,
        Dictionary<MeleeAttackType, IAttack> attacks) : base(position, material)
    {
        Attacks = attacks;
    }
}