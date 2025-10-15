using SadRogue.Integration;

namespace LuckNGold.World.Furniture.Interfaces;

/// <summary>
/// It can be interacted with (usually by standing next to it and pressing interact
/// keyboard shortcut).
/// </summary>
internal interface IInteractable
{
    /// <summary>
    /// Execute the interaction logic.
    /// </summary>
    /// <param name="interactor">Entity interacting with the component.</param>
    /// <returns>True if the interaction was successful, false otherwise.</returns>
    bool Interact(RogueLikeEntity interactor);
}