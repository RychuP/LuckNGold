using LuckNGold.World.Turns.Actions;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can move (change position).
/// </summary>
internal interface IMotion
{
    WalkAction GetWalkAction(Point destination);
}