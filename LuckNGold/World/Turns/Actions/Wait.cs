using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

/// <summary>
/// Passes turn by using time points on waiting.
/// </summary>
/// <param name="entity">Entity waiting.</param>
/// <param name="time">Time points to be spent waiting.</param>
internal class Wait(RogueLikeEntity entity, int time) : Action(entity, time)
{
    public override bool Execute() => true;
}