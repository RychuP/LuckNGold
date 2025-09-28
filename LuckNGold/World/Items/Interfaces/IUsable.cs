using SadRogue.Integration;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// Interface for components that provide functionality and can be used again in the future.
/// </summary>
/// <remarks>Should not be combined with <see cref="IConsumable"/>.</remarks>
internal interface IUsable : ICarryable
{
    /// <summary>
    /// Performs that functionality of the component.
    /// </summary>
    /// <param name="user">Entity activating the component.</param>
    /// <returns>True if the functionality was performed successfully, false otherwise.</returns>
    bool Activate(RogueLikeEntity user);
}