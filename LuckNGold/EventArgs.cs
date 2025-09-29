using LuckNGold.World.Items.Components;
using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;

namespace LuckNGold;

/// <summary>
/// Event raised by <see cref="InventoryComponent"/> when its contents change.
/// </summary>
/// <param name="item">Item added or removed from the <see cref="InventoryComponent"/>.</param>
class InventoryEventArgs(RogueLikeEntity item) : EventArgs()
{
    public RogueLikeEntity Item { get; } = item;
}

/// <summary>
/// Event raised by <see cref="QuickAccessComponent"/> when the items in slots change.
/// </summary>
/// <param name="index">Index of the slot.</param>
/// <param name="prevItem">Item previously occupying the slot.</param>
/// <param name="newItem">Item currently occupying the slot.</param>
class QuickAccessEventArgs(int index, RogueLikeEntity? prevItem, RogueLikeEntity? newItem) : 
    EventArgs()
{
    public RogueLikeEntity? PrevItem { get; } = prevItem;
    public RogueLikeEntity? NewItem { get; } = newItem;
    public int Index { get; } = index;
}