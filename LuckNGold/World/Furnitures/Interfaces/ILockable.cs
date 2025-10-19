using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can be locked. Must be unlocked before access is granted.
/// </summary>
internal interface ILockable
{
    /// <summary>
    /// Difficulty of the <see cref="ILockable"/>.
    /// Requires a matching index of <see cref="IUnlocker.Quality"/> to unlock.
    /// </summary>
    Difficulty Difficulty { get; }

    /// <summary>
    /// Tries to unlock the <see cref="ILockable"/> with the given <see cref="IUnlocker"/>.
    /// </summary>
    /// <param name="unlocker"><see cref="IUnlocker"/> being 
    /// used to unlock the <see cref="ILockable"/></param>
    bool Unlock(IUnlocker unlocker);
}