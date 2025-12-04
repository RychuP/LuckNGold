using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class AttackAction(RogueLikeEntity source, RogueLikeEntity target, int time) : 
    Action(source, time)
{
    public override bool Execute()
    {
        if (Source.AllComponents.GetFirstOrDefault<IBumpable>() is IBumpable sourceBumpable &&
            target.AllComponents.GetFirstOrDefault<IBumpable>() is IBumpable targetBumpable)
        {
            targetBumpable.OnBumped(Source);
            sourceBumpable.OnBumping(target);
            return true;
        }
        return false;
    }
}