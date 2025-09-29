using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// This component is meant to be attached to the Player. 
/// It allows a selection of items from the inventory to have keyboard shortcuts for ease of use.
/// </summary>
internal class QuickAccessComponent
{
    public event EventHandler<QuickAccessEventArgs>? SlotChanged;

    /// <summary>
    /// Max number of items to have quick access.
    /// </summary>
    public const int MaxItemsCount = 10;

    // Slots available to be assigned with items for quick access
    readonly RogueLikeEntity?[] _slots = new RogueLikeEntity[MaxItemsCount];

    public QuickAccessComponent(InventoryComponent inventory)
    {
        inventory.ItemAdded += Inventory_OnItemAdded;
        inventory.ItemRemoved += Inventory_OnItemRemoved;
    }

    public RogueLikeEntity? GetItem(int index) =>
        _slots[index];

    int GetNextEmptySlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] is null)
                return i;
        }
        return -1;
    }

    void Inventory_OnItemAdded(object? sender, InventoryEventArgs e)
    {
        int index = GetNextEmptySlot();
        if (index >= 0)
        {
            var prevItem = _slots[index];
            _slots[index] = e.Item;
            OnSlotChanged(index, prevItem, _slots[index]);
        }
    }

    void Inventory_OnItemRemoved(object? sender, InventoryEventArgs e)
    {
        int index = Array.IndexOf(_slots, e.Item);
        if (index >= 0)
        {
            var prevItem = _slots[index];
            _slots[index] = null;
            OnSlotChanged(index, prevItem, _slots[index]);
        }
    }

    void OnSlotChanged(int index, RogueLikeEntity? prevItem, RogueLikeEntity? newItem)
    {
        var args = new QuickAccessEventArgs(index, prevItem, newItem);
        SlotChanged?.Invoke(this, args);
    }
}