using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can react to bumps.
/// </summary>
public interface IBumpable
{
    /// <summary>
    /// Does whatever bump action is needed, using the given entity as the source.
    /// </summary>
    void OnBumped(RogueLikeEntity source);
}