using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

// TODO: Probably can be deleted
// Try to add IInteractable to the individual components that can be interacted with
// and use some sort of a selector if there is more than one?
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
            if (op.IsOpen)
                return op.Close();
            else
                return op.Open();
        }

        return false;
    }
}