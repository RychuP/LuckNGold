using SadRogue.Integration;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// Interface for components that can be consumed or otherwise used once and removed from game after.
/// </summary>
/// <remarks>Should not be combined with <see cref="IUsable"/>.</remarks>
internal interface IConsumable : ICarryable
{
    /// <summary>
    /// Performs the actual effect of the item.
    /// </summary>
    /// <param name="consumer">The entity consuming the consumable.</param>
    /// <returns>True if the consumption was successful; false otherwise.</returns>
    bool Consume(RogueLikeEntity consumer);
}