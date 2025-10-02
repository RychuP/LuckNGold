using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can react to bumps.
/// </summary>
public interface IBumpable
{
    /// <summary>
    /// Does whatever bump action is needed, using the given entity as the source.
    /// </summary>
    /// <returns>True if a bump action was taken, false otherwise.</returns>
    bool OnBumped(RogueLikeEntity source);
}