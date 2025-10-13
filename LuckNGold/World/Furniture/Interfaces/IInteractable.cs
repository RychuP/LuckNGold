using SadRogue.Integration;

namespace LuckNGold.World.Furniture.Interfaces;

/// <summary>
/// It can be interacted with (usually by standing next to it and pressing interact
/// keyboard shortcut).
/// </summary>
internal interface IInteractable
{
    bool Interact(RogueLikeEntity interactor);
}