using LuckNGold.World.Items.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that can be locked/unlocked.
/// </summary>
internal class LockComponent(KeyType keyType) 
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ILockable
{
    public KeyType KeyType { get; } = keyType;
    public bool IsLocked { get; private set; } = true;

    public bool Unlock(IKey key)
    {
        if (key.Type == KeyType)
        {
            IsLocked = false;
            return true;
        }
        return false;
    }
}