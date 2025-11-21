using LuckNGold.Resources;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Materials;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

static class ClothingFactory
{
    public static RogueLikeEntity LinenClothing() =>
        GetClothing(Fabric.Linen);

    static RogueLikeEntity GetClothing(IFabric material)
    {
        var clothing = ItemFactory.GetEquippableEntity($"{material} {Strings.ClothingTag}", EquipSlot.Body);
        clothing.AllComponents.Add(new CompositionComponent(material));
        return clothing;
    }
}