using GoRogue.GameFramework;
using LuckNGold.World.Map;
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
    Point _initialPosition = Point.None;
    Point _lastKnownPlayerPosition = Point.None;

    public IAction GetAction()
    {
        if (Parent is not RogueLikeEntity parent)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (parent.CurrentMap is not GameMap map)
            throw new InvalidOperationException("Parent needs to be on the map.");

        var timeTracker = Parent.AllComponents.GetFirst<ITimeTracker>();

        // Check if parent is inside player FOV.
        if (Parent.IsVisible)
        {
            // Get player.
            var player = map.Monsters
                .Where(m => m.Name == "Player")
                .First();

            // Save current player position.
            _lastKnownPlayerPosition = player.Position;

            // Get if the player position is reachable.
            if (map.AStar.ShortestPath(Parent.Position, player.Position)
                is GoRogue.Pathing.Path path && path.Length > 0)
            {
                var firstPoint = path.GetStep(0);
                if (Parent.CanMove(firstPoint))
                {
                    // Go towards player position.
                    var motionComponent = Parent.AllComponents.GetFirst<IMotion>();
                    return motionComponent.GetWalkAction(firstPoint);
                }
                else
                {
                    // Check if player is within attack reach.
                    if (player.Position == firstPoint)
                    {
                        if (Parent.AllComponents.GetFirstOrDefault<ICombatant>() 
                            is ICombatant combatant)
                        {
                            return combatant.GetMeleeAttackAction(player);
                        }
                        else
                        {
                            return timeTracker.GetWaitAction();
                        }
                    }
                    else
                    {
                        return timeTracker.GetWaitAction();
                    }
                }
            }
            else
            {
                return timeTracker.GetWaitAction();
            }
        }
        // Parent is outside player FOV.
        else
        {
            // Check if last known player position is set.
            if (_lastKnownPlayerPosition != Point.None)
            {
                // Check if the last known player position is reachable.
                if (map.AStar.ShortestPath(Parent.Position, _lastKnownPlayerPosition)
                    is GoRogue.Pathing.Path path && path.Length > 0)
                {
                    var firstPoint = path.GetStep(0);
                    if (Parent.CanMove(firstPoint))
                    {
                        // Go towards last known player position.
                        var motionComponent = Parent.AllComponents.GetFirst<IMotion>();
                        return motionComponent.GetWalkAction(firstPoint);
                    }
                    // Try going back to initial position.
                    else
                    {
                        return GetWalkHomeOrWaitAction();
                    }
                }
                // LKPP is not reachable.
                else
                {
                    // Forget LKPP and go back home.
                    _lastKnownPlayerPosition = Point.None;
                    return GetWalkHomeOrWaitAction();
                }
            }
            // LKPP is not set.
            else
            {
                // Check if parent is at home position.
                if (Parent.Position != _initialPosition)
                {
                    return GetWalkHomeOrWaitAction();
                }
                // Parent is back at home position.
                else
                {
                    return timeTracker.GetWaitAction();
                }
            }
        }

        IAction GetWalkHomeOrWaitAction()
        {
            // Try to get path home.
            if (map.AStar.ShortestPath(parent.Position, _initialPosition)
                is GoRogue.Pathing.Path path && path.Length > 0)
            {
                var firstPoint = path.GetStep(0);
                if (parent.CanMove(firstPoint))
                {
                    // Go towards home position.
                    var motionComponent = parent.AllComponents.GetFirst<IMotion>();
                    return motionComponent.GetWalkAction(firstPoint);
                }
                // Seems stuck. Wait.
                else
                {
                    return timeTracker.GetWaitAction();
                }
            }
            // Path is not valid. Wait.
            else
            {
                return timeTracker.GetWaitAction();
            }
        }
    }

    public override void OnAdded(IScreenObject host)
    {
        if (host is RogueLikeEntity entity)
        {
            entity.AddedToMap += RogueLikeEntity_OnAddedToMap;
        }

        base.OnAdded(host);
    }

    void RogueLikeEntity_OnAddedToMap(object? o, GameObjectCurrentMapChanged e)
    {
        if (o is not RogueLikeEntity entity)
            throw new InvalidOperationException();

        _initialPosition = entity.Position;
    }
}