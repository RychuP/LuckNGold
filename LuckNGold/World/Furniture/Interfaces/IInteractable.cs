using SadRogue.Integration;

namespace LuckNGold.World.Furniture.Interfaces;

internal interface IInteractable
{
    bool Interact(RogueLikeEntity interactor);
}