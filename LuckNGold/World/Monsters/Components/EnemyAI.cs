using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Simple component that moves its parent toward the player if the player is visible. It demonstrates the basic
/// usage of the integration library's component system, as well as basic AStar pathfinding.
/// </summary>
internal class EnemyAI() : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IEnemyAI
{
    public IAction TakeTurn()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var timeTracker = Parent.AllComponents.GetFirst<ITimeTracker>();
        return new PassTurn(Parent, timeTracker.Time);

        //if (Parent == null) return;

        //var map = Parent.CurrentMap;
        //if (map == null) return;

        //if (!map.PlayerFOV.CurrentFOV.Contains(Parent.Position)) return;

        //var path = map.AStar.ShortestPath(Parent.Position, Program.RootScreen!.Player.Position);
        //if (path == null) return;
        //var firstPoint = path.GetStep(0);
        //if (Parent.CanMove(firstPoint))
        //{
        //    var direction = Direction.GetDirection(Parent.Position, firstPoint);
        //    Program.RootScreen.MessageLog.AddMessage($"An enemy moves {direction}!");
        //    Parent.Position = firstPoint;
        //}
    }
}