using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that can gain experience and improve their level.
/// </summary>
internal class LevelComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ILevel
{
    public event EventHandler<ValueChangedEventArgs<int>>? ExperienceChanged;
    public event EventHandler<ValueChangedEventArgs<int>>? LevelChanged;

    int _experience = 0;
    public int Experience
    {
        get => _experience;
        set
        {
            if (_experience == value) return;
            if (value < _experience)
                throw new InvalidOperationException("Experience does not go down.");
            int prevExp = _experience;
            _experience = value;
            OnExperienceChanged(prevExp, value);
        }
    }

    int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            if (value == _level) return;
            if (value < _level)
                throw new InvalidOperationException("Level does not go down.");
            int prevLevel = _level;
            _level = value;
            OnLevelChanged(prevLevel, value);
        }
    }

    void OnExperienceChanged(int prevExp, int newExp)
    {
        var args = new ValueChangedEventArgs<int>(prevExp, newExp);
        ExperienceChanged?.Invoke(this, args);
    }

    void OnLevelChanged(int prevLevel, int newLevel)
    {
        var args = new ValueChangedEventArgs<int>(prevLevel, newLevel);
        LevelChanged?.Invoke(this, args);
    }
}