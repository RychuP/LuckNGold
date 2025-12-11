using LuckNGold.Config;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can allocate time points to perform actions.
/// </summary>
internal class TimeTrackerComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ITimeTracker
{
    // Unused...
    public event EventHandler<ValueChangedEventArgs<int>>? TimeChanged;

    public int Speed { get; set; } = GameSettings.TurnTime;

    int _time = GameSettings.TurnTime;
    public int Time
    {
        get => _time;
        set
        {
            if (_time == value) return;
            else if (value < 0) value = 0;
            int prevTime = _time;
            _time = value;
            OnTimeChanged(prevTime, value);
        }
    }

    public Wait GetWaitAction()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        return new Wait(Parent, Time);
    }

    void OnTimeChanged(int prevTime, int newTime)
    {
        var args = new ValueChangedEventArgs<int>(prevTime, newTime);
        TimeChanged?.Invoke(this, args);
    }
}