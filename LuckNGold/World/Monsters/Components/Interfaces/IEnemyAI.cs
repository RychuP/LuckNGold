using LuckNGold.World.Turns.Actions;

namespace LuckNGold.World.Monsters.Components.Interfaces;

internal interface IEnemyAI
{
    IAction GetAction();
}