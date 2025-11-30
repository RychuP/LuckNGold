using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class PassTurn(RogueLikeEntity entity, int time) : Action(entity, time)
{
    public override bool Execute() => true;
}