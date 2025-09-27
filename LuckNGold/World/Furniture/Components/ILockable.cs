using LuckNGold.World.Items.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Interface for components that can be locked/unlocked.
/// </summary>
internal interface ILockable
{
    bool IsLocked { get; }

    bool Unlock(IKey key);
}