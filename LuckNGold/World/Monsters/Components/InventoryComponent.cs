using LuckNGold.World.Items.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component representing an inventory which can hold a given number of items.
/// </summary>
internal class InventoryComponent(int capacity) 
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false)
{
    public event EventHandler<InventoryItemEventArgs>? ItemAdded;
    public event EventHandler<InventoryItemEventArgs>? ItemRemoved;

    public int Capacity { get; } = capacity;

    public readonly List<RogueLikeEntity> Items = new(capacity);

    /// <summary>
    /// Drops the given item from this inventory.
    /// </summary>
    public void Drop(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Can't drop an entity from an inventory " +
                "that's not connected to an object.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Objects are not allowed to drop items " +
                "from their inventory when they're not part of a map.");

        if (Remove(item))
        {
            item.Position = Parent.Position;
            Parent.CurrentMap.AddEntity(item);
        }
    }

    /// <summary>
    /// Tries to pick up the first item found at the Parent's location.
    /// </summary>
    /// <returns>True if an item was picked up, false otherwise.</returns>
    public bool PickUp()
    {
        if (Parent == null)
            throw new InvalidOperationException("Can't pick up an item into an inventory " +
                "that's not connected to an object.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Entity must be part of a map to pick up items.");

        foreach (var entity in Parent.CurrentMap.GetEntitiesAt<RogueLikeEntity>(Parent.Position))
        {
            // Check if any of the components of the entity allow it to be picked up
            if (!entity.AllComponents.Contains<ICarryable>()) 
                continue;

            if (Add(entity))
            {
                entity.CurrentMap!.RemoveEntity(entity);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Causes the parent to consume the given consumable item. 
    /// The given entity must have some component implementing IConsumable.
    /// </summary>
    public bool Consume(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Cannot consume item from an inventory " +
                "not attached to an object.");

        var consumable = item.AllComponents.GetFirst<IConsumable>();

        if (!consumable.Consume(Parent)) 
            return false;

        Remove(item);
        return true;
    }

    /// <summary>
    /// Adds an item to the inventory contents.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    /// <returns>True if success, false otherwise.</returns>
    bool Add(RogueLikeEntity item)
    {
        if (Items.Count >= Capacity || Items.Contains(item))
            return false;

        Items.Add(item);
        OnItemAdded(item);
        return true;
    }

    /// <summary>
    /// Removes an item from the inventory contents.
    /// </summary>
    /// <param name="item">Item to be removed.</param>
    /// <returns>True if success, false otherwise.</returns>
    bool Remove(RogueLikeEntity item)
    {
        if (!Items.Contains(item))
            return false;
        Items.Remove(item);
        OnItemRemoved(item);
        return true;
    }

    void OnItemAdded(RogueLikeEntity item)
    {
        var args = new InventoryItemEventArgs(item);
        ItemAdded?.Invoke(this, args);
    }

    void OnItemRemoved(RogueLikeEntity item)
    {
        var args = new InventoryItemEventArgs(item);
        ItemRemoved?.Invoke(this, args);
    }
}