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
    /// <param name="source">Entity that is the source of the bump.</param>
    void OnBumped(RogueLikeEntity source);

    /// <summary>
    /// Action taken by the entity that initiated the bumb.
    /// </summary>
    /// <param name="target">Entity that is the target of the bump.</param>
    void OnBumping(RogueLikeEntity target);
}