using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component representing an inventory which can hold a given number of items.
/// </summary>
internal class InventoryComponent(int capacity) 
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IInventory
{
    public event EventHandler<InventoryEventArgs>? ItemAdded;
    public event EventHandler<InventoryEventArgs>? ItemRemoved;

    public int Capacity { get; } = capacity;

    public List<RogueLikeEntity> Items { get; } = new(capacity);

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
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent must be part of a map to pick up items.");

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
    /// Tries to use the item.
    /// </summary>
    public bool Use(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.Layer != (int)GameMap.Layer.Monsters)
            throw new InvalidOperationException("Only entities from Monsters layer can use items.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Entity needs to be on the map to use items.");

        if (!Items.Contains(item))
            throw new InvalidOperationException("Item needs to be in the inventory to be used.");

        // Parent is a monster, on the map and has the item in their inventory. 
        // Let's try to use it. First check if it can be used.
        var usable = item.AllComponents.GetFirstOrDefault<IUsable>();
        if (usable == null)
            return false;

        if (!usable.Activate(Parent)) 
            return false;

        return true;
    }

    /// <summary>
    /// Adds an item to the inventory contents.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    /// <returns>True if success, false otherwise.</returns>
    public bool Add(RogueLikeEntity item)
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
    public bool Remove(RogueLikeEntity item)
    {
        if (!Items.Contains(item))
            return false;
        Items.Remove(item);
        OnItemRemoved(item);
        return true;
    }

    void OnItemAdded(RogueLikeEntity item)
    {
        var args = new InventoryEventArgs(item);
        ItemAdded?.Invoke(this, args);
    }

    void OnItemRemoved(RogueLikeEntity item)
    {
        var args = new InventoryEventArgs(item);
        ItemRemoved?.Invoke(this, args);
    }
}