using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

abstract class Action(RogueLikeEntity entity, int time) : IAction
{
    public RogueLikeEntity Entity { get; } = entity;
    public int Time { get; set; } = time;
    public virtual bool Execute() => false;
}