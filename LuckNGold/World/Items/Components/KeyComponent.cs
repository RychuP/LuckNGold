using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
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

    /// <summary>
    /// Checks the consumer and nearby entities 
    /// searching for a matching locked component that could be unlocked.
    /// </summary>
    /// <param name="consumer"></param>
    /// <returns></returns>
    public bool Consume(RogueLikeEntity consumer)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity " +
                "in order to be able to unlock other entities.");

        // Check the consumer is locked first 
        // TODO case like: door that wants to unlock itself ??
        // not sure if this case is necessary...
        // could a locked door be made target of this method from player's inventory?
        // not sure yet
        if (EntityHasLockable(consumer, out ILockable? lockable) && TryUnlock(lockable))
            return true;

        if (consumer.CurrentMap == null)
            return false;

        // start checking neighbours looking for locked entities (doors, chests, etc)
        var nearbyPoints = AdjacencyRule.EightWay.Neighbors(consumer.Position);
        foreach (var point in nearbyPoints)
        {
            var entities = consumer.CurrentMap.GetEntitiesAt<RogueLikeEntity>(point);
            foreach (var entity in entities)
            {
                if (EntityHasLockable(entity, out lockable) && TryUnlock(lockable))
                    return true;
            }
        }
        
        return false;
    }

    static bool EntityHasLockable(RogueLikeEntity entity, 
        [NotNullWhen(true)] out ILockable? lockable)
    {
        if (entity.AllComponents.GetFirst<ILockable>() is ILockable component)
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
}