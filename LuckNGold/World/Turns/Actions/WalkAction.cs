using GoRogue.GameFramework;
using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class WalkAction(int time, RogueLikeEntity source, Point destination) : 
    Action(source, time)
{
    public override bool Execute()
    {
        if (Source.CanMove(destination))
        {
            Source.Position = destination;
            return true;
        }
        else
            return false;
    }
}