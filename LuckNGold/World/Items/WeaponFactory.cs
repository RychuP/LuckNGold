using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class WeaponFactory
{
    public static RogueLikeEntity ArmingSword(IMaterial material, 
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Arming", material, attacks);

    public static RogueLikeEntity GladiusSword(IMaterial material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Gladius", material, attacks);

    public static RogueLikeEntity ScimitarSword(IMaterial material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks) =>
        Sword("Scimitar", material, attacks);

    static RogueLikeEntity Sword(string name, IMaterial material,
        Dictionary<MeleeAttackType, IAttackDamage> attacks)
    {
        var sword = ItemFactory.GetEquippableEntity($"{material} {name} Sword", EquipSlot.RightHand);
        sword.AllComponents.Add(new MeleeAttackComponent(attacks));
        sword.AllComponents.Add(new CompositionComponent(material));
        return sword;
    }
}