using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components.Interfaces;

namespace LuckNGold.World.Turns.Actions;

/// <summary>
/// It signifies the point in time when one turn ends and another begins.
/// </summary>
internal interface ITick : IEvent
{
    GameMap Map { get; }

    /// <summary>
    /// Replenishes time for all entities with <see cref="ITimeTracker"/>.
    /// </summary>
    void Reset();
}