using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Interfaces;
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

        var item = Items[index] ?? 
            throw new InvalidOperationException("No item at the given index.");

        Drop(item);
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

        var wallet = Parent.AllComponents.GetFirstOrDefault<WalletComponent>();

        foreach (var item in Parent.CurrentMap.GetEntitiesAt<RogueLikeEntity>(Parent.Position))
        {
            // Check if any of the components of the entity allow it to be picked up
            if (!item.AllComponents.Contains<ICarryable>()) 
                continue;

            if (item.AllComponents.GetFirstOrDefault<CurrencyComponent>() is 
                CurrencyComponent currency)
            {
                if (wallet is not null)
                {
                    wallet.Coins += currency.Amount;
                    item.CurrentMap!.RemoveEntity(item);
                }
            }

            else if (Add(item))
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
        if (!Items.Contains(item))
            throw new InvalidOperationException("Item is not present in the inventory.");

        return Activate(item);
    }

    public bool Use(int index)
    {
        if (index < 0 || index >= Items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var item = Items[index];
        return item is not null && Activate(item);
    }

    bool Activate(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        // Check item is usable
        var usable = item.AllComponents.GetFirstOrDefault<IUsable>();
        if (usable == null)
            return false;

        if (!usable.Activate(Parent))
            return false;

        return true;
    }

    public bool Equip(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        if (!Items.Contains(item))
            throw new InvalidOperationException("Item is not present in the inventory.");

        var equipmentComponent = Parent.AllComponents.GetFirst<IEquipment>();

        // Check item can be equipped.
        var equippableComponent = item.AllComponents.GetFirstOrDefault<IEquippable>();
        if (equippableComponent is null) 
            return false;

        // Try to equip the item.
        if (equipmentComponent.Equip(item, out RogueLikeEntity? unequippedItem))
        {
            // Remove item from inventory.
            Remove(item);

            // Add unequipped item, if any, to inventory.
            if (unequippedItem != null)
            {
                Add(unequippedItem);
            }

            return true;
        }

        return false;
    }
}