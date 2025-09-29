using SadRogue.Integration;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// Interface for components that can be used or consumed.
/// </summary>
internal interface IUsable : ICarryable
{
    /// <summary>
    /// Item is consumed and removed from the game if true, otherwise can be used again.
    /// </summary>
    bool IsSingleUse { get; }

    /// <summary>
    /// Performs that functionality of the component.
    /// </summary>
    /// <param name="user">Entity activating the component.</param>
    /// <returns>True if the functionality was performed successfully, false otherwise.</returns>
    bool Activate(RogueLikeEntity user);
}