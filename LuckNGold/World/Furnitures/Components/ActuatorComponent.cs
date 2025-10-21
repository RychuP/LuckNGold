using LuckNGold.World.Furnitures.Enums;
using LuckNGold.World.Furnitures.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that need an actuator mechanism to operate some of its parts.
/// </summary>
internal class ActuatorComponent() : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IActuator
{
    public event EventHandler? StateChanged;

    ActuatorState _state;
    public ActuatorState State
    {
        get => _state;
        private set
        {
            if (_state == value) return;
            _state = value;
            OnStateChanged();
        }
    }

    public void Extend()
    {
        State = ActuatorState.Extended;
    }

    public void Retract()
    {
        State = ActuatorState.Retracted;
    }

    void OnStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}