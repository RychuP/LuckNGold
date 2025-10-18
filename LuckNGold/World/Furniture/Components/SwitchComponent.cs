using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that can switch between on and off state.
/// </summary>
internal class SwitchComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ISwitch
{
    public event EventHandler<ValueChangedEventArgs<SwitchState>>? StateChanged;

    SwitchState _state = SwitchState.Off;
    /// <inheritdoc/>
    public SwitchState State
    {
        get => _state;
        private set
        {
            if (_state == value) return;
            var prevVal = _state;
            _state = value;
            OnStateChanged(prevVal, value);
        }
    }

    public bool Interact(RogueLikeEntity interactor)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture and below needs to be on the map.");

        Toggle();
        return true;
    }

    public void Toggle()
    {
        State = State == SwitchState.Off ? SwitchState.On : SwitchState.Off;
    }

    void OnStateChanged(SwitchState prevState, SwitchState newState)
    {
        var args = new ValueChangedEventArgs<SwitchState>(prevState, newState);
        StateChanged?.Invoke(this, args);
    }
}