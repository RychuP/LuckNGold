using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class WeaponFactory
{
    public static RogueLikeEntity ArmingSword(Material material, 
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Arming", material, attacks);

    public static RogueLikeEntity GladiusSword(Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Gladius", material, attacks);

    public static RogueLikeEntity ScimitarSword(Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Scimitar", material, attacks);

    static RogueLikeEntity Sword(string name, Material material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks)
    {
        var sword = ItemFactory.GetEntity($"{material} {name} Sword");
        sword.AllComponents.Add(new EquippableComponent(EquipSlot.RightHand));
        sword.AllComponents.Add(new MeleeAttackComponent(attacks));
        sword.AllComponents.Add(new MaterialComponent(material));
        return sword;
    }
}