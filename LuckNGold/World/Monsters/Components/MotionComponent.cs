using LuckNGold.Config;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

internal class MotionComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IMotion
{
    public int GetMoveCost()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<IEquipment>();

        // Return simplified move cost for now.
        return GameSettings.TurnTime;
    }
}