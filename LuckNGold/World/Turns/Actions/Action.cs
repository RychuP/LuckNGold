using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

abstract class Action(RogueLikeEntity source, int time) : IAction
{
    public RogueLikeEntity Source { get; } = source;
    public int Time { get; set; } = time;
    public virtual bool Execute() => false;
}