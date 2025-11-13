using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.Generation.Items.Weapons.Swords;

abstract record Sword : MeleeWeapon
{
    public Sword (Point position, Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) : base(position, material, attacks)
    {

    }
}