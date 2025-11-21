using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Materials;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class ArmourFactory
{
    public static RogueLikeEntity BanditHelmet(IMaterial material) =>
        GetHelmet("Bandit", material);


    public static RogueLikeEntity BanditShield(IMaterial material) =>
        GetShield("Bandit", material);

    static RogueLikeEntity GetHelmet(string name, IMaterial material)
    {
        var helmet = ItemFactory.GetEquippableEntity($"{material} {name} Helmet", EquipSlot.Head);
        helmet.AllComponents.Add(new CompositionComponent(material));
        return helmet;
    }

    static RogueLikeEntity GetShield(string name, IMaterial material)
    {
        var shield = ItemFactory.GetEquippableEntity($"{material} {name} Shield", EquipSlot.LeftHand);
        shield.AllComponents.Add(new CompositionComponent(material));
        return shield;
    }
}