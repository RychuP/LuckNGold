using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

/// <summary>
/// It can be performed by an entity.
/// </summary>
internal interface IAction : IEvent
{
    /// <summary>
    /// Entity taking the <see cref="IAction"/>.
    /// </summary>
    RogueLikeEntity Source { get; }

    /// <summary>
    /// Executes <see cref="IAction"/>.
    /// </summary>
    /// <returns>True if <see cref="IAction"/> executed successfully, false otherwise.</returns>
    bool Execute();
}