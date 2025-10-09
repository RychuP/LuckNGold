using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that has no particular use but is considered valuable.
/// </summary>
internal class CollectableComponent(int value, int maxStackSize) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IStackable, IValuable
{
    public int MaxStackSize => maxStackSize;
    public int Value => value;
}