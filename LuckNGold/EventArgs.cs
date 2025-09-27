using LuckNGold.World.Items.Components;
using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;

namespace LuckNGold;

/// <summary>
/// Event raised by <see cref="InventoryComponent"/> when its contents change.
/// </summary>
/// <param name="item">Item added or removed from the <see cref="InventoryComponent"/>.</param>
class InventoryItemEventArgs(RogueLikeEntity item) : EventArgs()
{
    public RogueLikeEntity Item { get; } = item;
}