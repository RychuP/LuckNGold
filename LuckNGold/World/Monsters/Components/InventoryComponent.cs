using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component representing an inventory which can hold a given number of items.
/// </summary>
internal class InventoryComponent(int capacity) : InventoryBase(capacity)
{
    /// <summary>
    /// Drops the given item from this inventory.
    /// </summary>
    public void Drop(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        if (Remove(item))
        {
            item.Position = Parent.Position;
            Parent.CurrentMap.AddEntity(item);
        }
    }

    public void Drop(int index)
    {
        if (index < 0 || index >= Items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        var item = Items[index] ?? 
            throw new InvalidOperationException("No item at the given index.");

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
        if (IsFull())
            return false;

        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        foreach (var item in Parent.CurrentMap.GetEntitiesAt<RogueLikeEntity>(Parent.Position))
        {
            // Check if any of the components of the entity allow it to be picked up
            if (!item.AllComponents.Contains<ICarryable>()) 
                continue;

            if (Add(item))
            {
                item.CurrentMap!.RemoveEntity(item);
                return true;
            }
        }

        return false;
    }

    public bool PickUp(RogueLikeEntity item)
    {
        if (IsFull())
            return false;

        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        if (item.Position != Parent.Position)
            throw new InvalidOperationException("Item and Parent need to have the same position.");

        if (Add(item))
        {
            item.CurrentMap!.RemoveEntity(item);
            return true;
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

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        if (!Items.Contains(item))
            throw new InvalidOperationException("Item needs to be in the inventory to be used.");

        return Activate(item);
    }

    public bool Use(int index)
    {
        if (index < 0 || index >= Items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        var item = Items[index];
        return item is not null && Activate(item);
    }

    bool Activate(RogueLikeEntity item)
    {
        // Check item is usable
        var usable = item.AllComponents.GetFirstOrDefault<IUsable>();
        if (usable == null)
            return false;

        if (!usable.Activate(Parent))
            return false;

        return true;
    }
}