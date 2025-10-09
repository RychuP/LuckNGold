using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be used in crafting recepies.
/// </summary>
/// <param name="material"></param>
internal class IngredientComponent(Material material, int maxStackSize) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IIngredient
{
    public int MaxStackSize => maxStackSize;
    public Material Material => material;
}