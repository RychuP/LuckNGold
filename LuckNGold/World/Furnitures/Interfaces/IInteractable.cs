using SadRogue.Integration;

namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can be interacted with. 
/// </summary>
internal interface IInteractable
{
    /// <summary>
    /// Execute the interaction logic.
    /// </summary>
    /// <param name="interactor">Entity interacting with the component.</param>
    void Interact(RogueLikeEntity interactor);
}