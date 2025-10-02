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
    public event EventHandler<InventoryEventArgs>? TreasureAdded;
    public event EventHandler<InventoryEventArgs>? TreasureRemoved;

    /// <inheritdoc/>
    public int Capacity { get; } = capacity;

    /// <inheritdoc/>
    public int Value { get; private set; }

    /// <inheritdoc/>
    public List<RogueLikeEntity> Items { get; } = new(capacity);

    /// <inheritdoc/>
    public List<RogueLikeEntity> Treasure { get; } = [];

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

    /// <inheritdoc/>
    public bool Add(RogueLikeEntity entity)
    {
        // Check item is a treasure
        if (entity.AllComponents.Contains<ITreasure>())
        {
            if (Treasure.Contains(entity))
                throw new InvalidOperationException("Trying to add a treasure which is already " +
                    "in the inventory");

            Treasure.Add(entity);
            OnTreasureAdded(entity);
            return true;
        }

        // Regular item
        else
        {
            if (Items.Count >= Capacity)
                return false;

            if (Items.Contains(entity))
                throw new InvalidOperationException("Trying to add an item which is already " +
                    "in the inventory");

            Items.Add(entity);
            OnItemAdded(entity);
            return true;
        }
        
    }

    /// <inheritdoc/>
    public bool Remove(RogueLikeEntity entity)
    {
        if (Items.Remove(entity))
        {
            OnItemRemoved(entity);
            return true;
        }
        else if (Treasure.Remove(entity))
        {
            OnTreasureRemoved(entity);
            return true;
        }
        return false;
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

    void OnTreasureAdded(RogueLikeEntity treasure)
    {
        Value += treasure.AllComponents.GetFirst<ITreasure>().Value;
        var args = new InventoryEventArgs(treasure);
        TreasureAdded?.Invoke(this, args);
    }

    void OnTreasureRemoved(RogueLikeEntity treasure)
    {
        Value -= treasure.AllComponents.GetFirst<ITreasure>().Value;
        var args = new InventoryEventArgs(treasure);
        TreasureRemoved?.Invoke(this, args);
    }
}