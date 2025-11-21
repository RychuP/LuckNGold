using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Base implementation of <see cref="IInventory"/> interface. 
/// </summary>
abstract class InventoryBase(int capacity)
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IInventory
{
    /// <summary>
    /// Event raised when contents of the inventory change.
    /// </summary>
    public event EventHandler<InventoryChangedEventArgs>? Changed;

    readonly List<RogueLikeEntity?> _items = 
        [.. Enumerable.Repeat<RogueLikeEntity?>(null, capacity)];

    /// <inheritdoc/>
    public int Capacity { get; } = capacity;

    /// <inheritdoc/>
    public ReadOnlyCollection<RogueLikeEntity?> Items => _items.AsReadOnly();

    /// <inheritdoc/>
    public bool Add(RogueLikeEntity entity)
    {
        int slot = GetNextEmptySlot();
        if (slot == -1)
            return false;

        return TryAddAt(slot, entity);
    }

    /// <inheritdoc/>
    public bool TryAddAt(int index, RogueLikeEntity item)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (item.AllComponents.GetFirstOrDefault<ICarryable>() is null)
            throw new InvalidOperationException("Item is not carryable.");

        if (_items.Contains(item))
            throw new InvalidOperationException("Item is in the inventory already.");

        if (_items[index] is not null)
            throw new InvalidOperationException("Slot is not empty.");

        var prevItem = _items[index];
        _items[index] = item;
        OnChanged(index, prevItem, item);

        return true;
    }

    /// <inheritdoc/>
    public bool Remove(RogueLikeEntity item)
    {
        int index = _items.IndexOf(item);
        if (index == -1)
            throw new InvalidOperationException("Item is not in the inventory.");

        _items[index] = null;
        OnChanged(index, item, null);

        return true;
    }

    public bool TryRemoveAt(int index, [NotNullWhen(true)] out RogueLikeEntity? item)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (_items[index] is not RogueLikeEntity itemToBeRemoved)
            throw new InvalidOperationException("No item at the given index.");

        item = itemToBeRemoved;
        _items[index] = null;
        OnChanged(index, item, null);

        return true;
    }

    public bool IsFull() =>
        !_items.Any(e => e is null);

    void OnChanged(int index, RogueLikeEntity? prevItem, RogueLikeEntity? newItem)
    {
        var args = new InventoryChangedEventArgs(index, prevItem, newItem);
        Changed?.Invoke(this, args);
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (Parent!.Layer != (int)GameMap.Layer.Monsters)
            throw new InvalidOperationException("Component is meant to be added " +
                "to a Monster entity.");
    }

    int GetNextEmptySlot() =>
        _items.FindIndex(e => e is null);
}

/// <summary>
/// Event raised when items in the inventory change.
/// </summary>
/// <param name="index">Index of the slot.</param>
/// <param name="prevItem">Item previously occupying the slot.</param>
/// <param name="newItem">Item currently occupying the slot.</param>
class InventoryChangedEventArgs(int index, RogueLikeEntity? prevItem, RogueLikeEntity? newItem) :
    EventArgs()
{
    public RogueLikeEntity? PrevItem { get; } = prevItem;
    public RogueLikeEntity? NewItem { get; } = newItem;
    public int Index { get; } = index;
}