using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity to indicate the composition of materials the item is made of.
/// </summary>
/// <param name="material">Material that the item entity is made of.</param>
internal class CompositionComponent(IMaterial material) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IComposition
{
    public IMaterial Material { get; } = material;
}