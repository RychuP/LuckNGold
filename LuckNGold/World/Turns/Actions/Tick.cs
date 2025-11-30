using LuckNGold.Config;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

internal class Tick(GameMap map) : ITick
{
    public int Time { get; set; } = GameSettings.TurnTime;

    public GameMap Map { get; } = map;

    public void Reset()
    {
        var timedEntities = Map.Entities
            .Select(el => el.Item)
            .Cast<RogueLikeEntity>()
            .Where(e => e.AllComponents.GetFirstOrDefault<ITimeTracker>() != null);

        foreach (var entity in timedEntities)
        {
            var timeTracker = entity.AllComponents.GetFirst<ITimeTracker>();
            timeTracker.Time = timeTracker.Speed;
        }
    }
}