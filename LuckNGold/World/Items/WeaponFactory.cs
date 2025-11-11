using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Items.Primitives;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class WeaponFactory
{
    public static RogueLikeEntity ArmingSword(Material material)
    {
        var overheadSwingDamage = new AttackDamage(PhysicalDamage.Slashing(3, 6), ElementalDamage.None);
        var sideSwingDamage = new AttackDamage(PhysicalDamage.Slashing(2, 5), ElementalDamage.None);
        var thrustDamage = new AttackDamage(PhysicalDamage.Piercing(1, 4), ElementalDamage.None);

        var attacks = new Dictionary<MeleeAttackType, IAttackDamage>()
        {
            { MeleeAttackType.OverheadSwing, overheadSwingDamage },
            { MeleeAttackType.DiagonalSideSwing, sideSwingDamage },
            { MeleeAttackType.ForwardThrust, thrustDamage }
        };

        string name = $"{material} Arming Sword";

        var sword = Sword(name, material, attacks);
        return sword;
    }

    static RogueLikeEntity Sword(string name, Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks)
    {
        var sword = ItemFactory.GetEntity(name);
        sword.AllComponents.Add(new EquippableComponent(EquipSlot.RightHand));
        sword.AllComponents.Add(new MeleeAttackComponent(attacks));
        sword.AllComponents.Add(new MaterialComponent(material));
        return sword;
    }
}