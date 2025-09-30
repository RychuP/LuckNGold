using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

internal class InteractableComponent : RogueLikeComponentBase<RogueLikeEntity>, IInteractable
{
    public InteractableComponent() : base(false, false, false, false)
    {

    }

    public bool Interact(RogueLikeEntity interactor)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        // Anything static and big must be present on the map to be interactable
        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture and below needs to be on the map.");

        // Look for known components that can be interacted with
        if (Parent.AllComponents.GetFirstOrDefault<OpeningComponent>() is OpeningComponent op)
        {
            PlayerFOVController playerFOV = interactor.AllComponents
                .GetFirst<PlayerFOVController>();

            if (op.IsOpen)
            {
                playerFOV.CalculateFOV();
                return op.Close();
            }
            else
            {
                playerFOV.CalculateFOV();
                return op.Open();
            }
        }

        return false;
    }
}