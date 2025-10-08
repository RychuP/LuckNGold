using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be used to open locked entities.
/// </summary>
internal class UnlockingComponent(Quality quality)
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IUnlocker
{
    /// <inheritdoc/>
    public Quality Quality { get; } = quality;

    /// <inheritdoc/>
    public bool IsSingleUse { get; } = true;

    // Checks if an entity has a lockable component
    static bool EntityHasLockable(RogueLikeEntity entity, 
        [NotNullWhen(true)] out ILockable? lockable)
    {
        if (entity.AllComponents.GetFirstOrDefault<ILockable>() is ILockable component)
        {
            lockable = component;
            return true;
        }
        lockable = null;
        return false;
    }

    bool TryUnlock(ILockable lockable)
    {
        if (lockable.Unlock(this))
            return true;
        return false;
    }

    /// <summary>
    /// Searches for nearby entities with <see cref="ILockable"/> that can be unlocked.
    /// </summary>
    /// <param name="user">Entity that is using the component.</param>
    /// <returns>True if managed to unlock something, false otherwise.</returns>
    public bool Activate(RogueLikeEntity user)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (user.CurrentMap == null)
            throw new InvalidOperationException("User has to be on the map to activate items.");

        // Find user's inventory holding the parent
        var inventories = user.AllComponents.GetAll<IInventory>();
        IInventory? inventory = null;
        foreach (var inv in inventories)
        {
            if (inv.Items.Contains(Parent))
            {
                inventory = inv;
                break;
            }
        }
        if (inventory is null)
            throw new InvalidOperationException("Unlocker is not in the user's inventory.");

        // Start checking user's neighbours looking for locked entities (doors, chests, etc)
        var nearbyPoints = AdjacencyRule.EightWay.Neighbors(user.Position);
        foreach (var point in nearbyPoints)
        {
            var entities = user.CurrentMap.GetEntitiesAt<RogueLikeEntity>(point);
            foreach (var entity in entities)
            {
                if (EntityHasLockable(entity, out ILockable? lockable) && TryUnlock(lockable))
                {
                    // Check if can be reused
                    if (IsSingleUse)
                    {
                        // Remove from the game if not
                        inventory.Remove(Parent);
                    }
                    return true;
                }
            }
        }

        return false;
    }
}