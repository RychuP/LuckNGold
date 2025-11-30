using LuckNGold.Config;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can allocate time points to perform actions.
/// </summary>
internal class TimeTrackerComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ITimeTracker
{
    public int Speed { get; set; } = GameSettings.TurnTime;
    public int Time { get; set; } = GameSettings.TurnTime;
}