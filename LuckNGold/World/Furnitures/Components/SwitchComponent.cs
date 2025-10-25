using LuckNGold.Visuals;
using LuckNGold.World.Furnitures.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that can switch between on and off state.
/// </summary>
internal class SwitchComponent(string onAnimation = "", string offAnimation = "",
    string turningOnAnimation = "", string turningOffAnimation = "",
    string abortTurningOnAnimation = "", string abortTurningOffAnimation = "") :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ISwitch
{
    public event EventHandler? StateChanged;
    public event EventHandler<ValueChangedEventArgs<bool>>? StateChanging;

    bool _stateChangeCanGoAhead = false;

    bool _isOn = false;
    /// <inheritdoc/>
    public bool IsOn
    {
        get => _isOn;
        private set
        {
            if (_isOn == value) return;
            _isOn = value;
            OnStateChanged();
        }
    }

    /// <inheritdoc/>
    public void Interact(RogueLikeEntity interactor)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        if (IsOn)
            TurnOff();
        else
            TurnOn();
    }

    void TurnOn()
    {
        if (IsOn) return;

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animatedEntity)
        {
            // Refuse to act if a turning animation is already playing.
            if (!animatedEntity.IsPlaying)
            {
                OnStateChanging(true);
                if (!_stateChangeCanGoAhead)
                {
                    animatedEntity.PlayAnimation(abortTurningOnAnimation);
                    return;
                }

                // Play turning animation.
                animatedEntity.PlayAnimation(turningOnAnimation);

                // Unlike other animated components, we need an immediate response.
                IsOn = true;

                // Default animation has to be changed so that out of view appearance is correct.
                animatedEntity.DefaultAnimation = onAnimation;
            }
        }
        else
        {
            OnStateChanging(true);
            if (!_stateChangeCanGoAhead)
                return;

            IsOn = true;
        }
    }

    void TurnOff()
    {
        if (!IsOn) return;

        // Check if parent is animated.
        if (Parent is AnimatedRogueLikeEntity animatedEntity)
        {
            // Refuse to act if a turning animation is already playing.
            if (!animatedEntity.IsPlaying)
            {
                // Announce IsOn is about to change state.
                OnStateChanging(false);
                if (!_stateChangeCanGoAhead)
                {
                    animatedEntity.PlayAnimation(abortTurningOffAnimation);
                    return;
                }

                // Play turning animation.
                animatedEntity.PlayAnimation(turningOffAnimation);

                // Unlike other animated components, we need an immediate response.
                IsOn = false;

                // Default animation has to be changed so that out of view appearance is correct.
                animatedEntity.DefaultAnimation = offAnimation;
            }
        }
        else
        {
            // Announce IsOn is about to change state.
            OnStateChanging(false);
            if (!_stateChangeCanGoAhead)
                return;

            IsOn = false;
        }
    }

    /// <summary>
    /// Stops the <see cref="IsOn"/> state changing when it is about to happen.
    /// </summary>
    public void StopStateChanging() =>
        _stateChangeCanGoAhead = false;

    void OnStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    void OnStateChanging(bool desiredIsOnState)
    {
        _stateChangeCanGoAhead = true;
        var args = new ValueChangedEventArgs<bool>(!desiredIsOnState, desiredIsOnState);
        StateChanging?.Invoke(this, args);
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (host is AnimatedRogueLikeEntity animatedEntity)
        {
            if (!animatedEntity.HasAnimation(onAnimation) ||
                !animatedEntity.HasAnimation(offAnimation) ||
                !animatedEntity.HasAnimation(turningOnAnimation) ||
                !animatedEntity.HasAnimation(turningOffAnimation) ||
                !animatedEntity.HasAnimation(abortTurningOnAnimation) ||
                !animatedEntity.HasAnimation(abortTurningOffAnimation))
                throw new InvalidOperationException("Host is missing switch animations.");

            animatedEntity.Finished += AnimatedRogueLikeEntity_OnFinished;
        }
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);

        if (host is AnimatedRogueLikeEntity animatedEntity)
        {
            animatedEntity.Finished -= AnimatedRogueLikeEntity_OnFinished;
        }
    }

    void AnimatedRogueLikeEntity_OnFinished(object? o, EventArgs e)
    {
        if (o is AnimatedRogueLikeEntity animatedEntity)
        {
            if (animatedEntity.CurrentAnimation == turningOnAnimation)
            {
                animatedEntity.PlayAnimation(onAnimation);
            }
            else if (animatedEntity.CurrentAnimation == turningOffAnimation)
            {
                animatedEntity.PlayAnimation(offAnimation);
            }
        }
    }
}