using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that needs to know what material it is made of.
/// </summary>
/// <param name="material">Material that the item entity is made of.</param>
internal class MaterialComponent(Material material) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IMaterial
{
    public Material Material { get; } = material;
}