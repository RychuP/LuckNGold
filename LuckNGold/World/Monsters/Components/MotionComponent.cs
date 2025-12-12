using LuckNGold.Config;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

internal class MotionComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IMotion
{
    public WalkAction GetWalkAction(Point destination)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var race = Parent.AllComponents.GetFirst<IIdentity>().Race;

        // Calculate move cost.
        int moveCost = race.BaseMoveCost;

        // Create the walk action.
        return new WalkAction(moveCost, Parent, destination);
    }
}