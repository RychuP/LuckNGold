using GoRogue.GameFramework;
using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class WalkAction(int time, RogueLikeEntity entity, Point destination) : 
    Action(entity, time)
{
    public override bool Execute()
    {
        if (Entity.CanMove(destination))
        {
            Entity.Position = destination;
            return true;
        }
        else
            return false;
    }
}