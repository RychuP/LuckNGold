using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.Generation.Items.Weapons.Swords;

abstract record Sword : MeleeWeapon
{
    public Sword (Point position, IMaterial material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) : base(position, material, attacks)
    {

    }
}