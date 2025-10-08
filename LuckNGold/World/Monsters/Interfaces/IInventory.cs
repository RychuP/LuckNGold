using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using System.Collections.ObjectModel;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can store entities with <see cref="ICarryable"/> component.
/// </summary>
internal interface IInventory
{
    /// <summary>
    /// Maximum amount of entities that can be added to <see cref="Items"/>.
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// List of all items held in the inventory.
    /// </summary>
    ReadOnlyCollection<RogueLikeEntity?> Items { get; }

    /// <summary>
    /// Removes the item from the inventory.
    /// </summary>
    /// <param name="item">Item being removed.</param>
    /// <returns>True if the item was removed from the inventory, false otherwise.</returns>
    bool Remove(RogueLikeEntity item);

    /// <summary>
    /// Tries to remove an item from the inventory at the given index.
    /// </summary>
    /// <param name="index">Index of the item to be removed.</param>
    /// <param name="item">Removed item it there was one present at the given index.</param>
    /// <returns>True if there was a valid item at the given index and it was removed, 
    /// false otherwise.</returns>
    bool TryRemoveAt(int index, out RogueLikeEntity? item);

    /// <summary>
    /// Adds an item to the inventory at the next available slot.
    /// </summary>
    /// <param name="item">Item being added.</param>
    /// <returns>True if item was added to the inventory, false otherwise.</returns>
    bool Add(RogueLikeEntity item);

    /// <summary>
    /// Tries to add an item to the inventory at the given index.
    /// </summary>
    /// <param name="index">Inventory index where the item needs to be added.</param>
    /// <param name="item">Item to be added.</param>
    /// <returns>True if the slot with the given index was empty and the item was added,
    /// false otherwise.</returns>
    bool TryAddAt(int index, RogueLikeEntity item);

    bool IsFull();
}