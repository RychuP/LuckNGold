using LuckNGold.Resources;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class FootwearFactory
{
    public static RogueLikeEntity PeasantShoes(IMaterial material) =>
        Shoes(Strings.PeasantTag, material);

    static RogueLikeEntity Shoes(string name, IMaterial material)
    {
        var shoes = ItemFactory.GetEquippableEntity($"{material} {name} Shoes", EquipSlot.Feet);
        shoes.AllComponents.Add(new CompositionComponent(material));
        return shoes;
    }
}