using SadRogue.Integration;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Interface for items that can be consumed.
/// </summary>
internal interface IConsumable : ICarryable
{
    /// <summary>
    /// Performs the actual effect of the item.
    /// </summary>
    /// <param name="consumer">The entity consuming the consumable.</param>
    /// <returns>True if the consumption was successful; false otherwise.</returns>
    bool Consume(RogueLikeEntity consumer);
}