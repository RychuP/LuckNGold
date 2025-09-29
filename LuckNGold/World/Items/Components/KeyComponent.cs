using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for entities that can be used to open locked entities.
/// </summary>
internal class KeyComponent(KeyColor keyColor)
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IKey
{
    /// <inheritdoc/>
    public KeyColor KeyColor { get; } = keyColor;

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
        if (lockable.IsLocked && lockable.Unlock(this))
            return true;
        return false;
    }

    /// <summary>
    /// The entity with the key component will search for nearby entities 
    /// like doors, chests, etc that can be unlocked.
    /// </summary>
    /// <param name="user">Entity that is using the key.</param>
    /// <returns>True if key managed to unlock something, false otherwise.</returns>
    public bool Activate(RogueLikeEntity user)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (user.CurrentMap == null)
            throw new InvalidOperationException("Entity needs to be on the map to activate items.");

        var inventory = user.AllComponents.GetFirst<IInventory>();
        if (!inventory.Items.Contains(Parent))
            throw new InvalidOperationException("User needs to have the key in their inventory.");

        // Start checking user's neighbours looking for locked entities (doors, chests, etc)
        var nearbyPoints = AdjacencyRule.EightWay.Neighbors(user.Position);
        foreach (var point in nearbyPoints)
        {
            var entities = user.CurrentMap.GetEntitiesAt<RogueLikeEntity>(point);
            foreach (var entity in entities)
            {
                if (EntityHasLockable(entity, out ILockable? lockable) && TryUnlock(lockable))
                {
                    // Matching lock was found and unlocked using this key
                    if (IsSingleUse)
                    {
                        // Key was used and is removed from the game
                        inventory.Remove(Parent);
                    }
                    return true;
                }
            }
        }

        return false;
    }
}