using LuckNGold.World.Furniture.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for entities that can be used to lock/unlock other entities.
/// </summary>
internal class KeyComponent(KeyType keyType)
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IConsumable, IKey
{
    public KeyType Type { get; } = keyType;

    public bool Consume(RogueLikeEntity consumer)
    {
        if (consumer.AllComponents.GetFirstOrDefault<ILockable>() is not ILockable lockable)
            throw new InvalidOperationException("Key cannot be consumed by an entity " +
                "that has not got a component that can be unlocked.");

        if (!lockable.IsLocked)
            return false;

        if (lockable.Unlock(this))
            return true;
        
        return false;
    }
}