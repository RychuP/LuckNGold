using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that have hit points, can die when they go below zero
/// and can suffer or benefit from transient conditions (like confused or hastened).
/// </summary>
/// <param name="health"></param>
internal class HealthComponent(int health) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IHealth
{
    public event EventHandler<ValueChangedEventArgs<int>>? HPChanged;

    int _hp = health;
    public int HP
    {
        get => _hp;
        set
        {
            if (_hp == value) return;
            int prevHP = _hp;
            _hp = value;
            OnHPChanged(prevHP, value);
        }
    }

    void OnHPChanged(int prevHP, int newHP)
    {
        var args = new ValueChangedEventArgs<int>(prevHP, newHP);
        HPChanged?.Invoke(this, args);
    }
}