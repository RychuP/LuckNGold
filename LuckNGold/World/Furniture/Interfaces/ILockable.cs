using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Furniture.Interfaces;

/// <summary>
/// Interface for components that can be locked/unlocked.
/// </summary>
internal interface ILockable
{
    /// <summary>
    /// Color of the key that unlocks this lock.
    /// </summary>
    KeyColor KeyColor { get; }

    /// <summary>
    /// Whether the component is locked or open.
    /// </summary>
    bool IsLocked { get; }

    /// <summary>
    /// Check the key for <see cref="KeyColor"/> and unlocks the component if there is a match.
    /// </summary>
    /// <param name="key">Key trying to open the lock.</param>
    /// <returns>True if the key managed to open the lock, false otherwise.</returns>
    bool Unlock(IKey key);
}