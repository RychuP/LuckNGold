using LuckNGold.Config;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class MeleeAttackAction(RogueLikeEntity source, RogueLikeEntity target, int time) : 
    Action(source, time)
{
    public override bool Execute()
    {
        var distance = GameSettings.Distance.Calculate(Source.Position, target.Position);

        if (distance == 1)
        {
            if (Source.AllComponents.GetFirstOrDefault<IBumpable>() is IBumpable sourceBumpable &&
            target.AllComponents.GetFirstOrDefault<IBumpable>() is IBumpable targetBumpable)
            {
                targetBumpable.OnBumped(Source);
                sourceBumpable.OnBumping(target);
                return true;
            }
            else
                throw new InvalidOperationException("IBumpable components are missing.");
        }
        else 
            return false;
    }
}