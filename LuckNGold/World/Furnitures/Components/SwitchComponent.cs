using LuckNGold.Visuals;
using LuckNGold.World.Furnitures.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that can switch between on and off state.
/// </summary>
internal class SwitchComponent(string onAnimation = "", string offAnimation = "",
    string turningOnAnimation = "", string turningOffAnimation = "") :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ISwitch
{
    //public event EventHandler<ValueChangedEventArgs<SwitchState>>? StateChanged;
    public event EventHandler? StateChanged;

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
    public bool Interact(RogueLikeEntity interactor)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture and below needs to be on the map.");

        if (IsOn)
            TurnOff();
        else
            TurnOn();

        return true;
    }

    void TurnOn()
    {
        if (IsOn) return;

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animated)
        {
            // Refuse to act if a turning animation is already playing.
            if (!animated.IsPlaying)
            {
                // Play turning animation.
                // Actual state change will happen in the animation changed event handler.
                animated.PlayAnimation(turningOnAnimation);
            }

            return;
        }

        IsOn = true;
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
                // Play turning animation.
                // Actual state change will happen in the animation changed event handler.
                animatedEntity.PlayAnimation(turningOffAnimation);
            }

            return;
        }

        IsOn = false;
    }

    void OnStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (host is AnimatedRogueLikeEntity animatedEntity)
        {
            if (!animatedEntity.HasAnimation(onAnimation) ||
                !animatedEntity.HasAnimation(offAnimation) ||
                !animatedEntity.HasAnimation(turningOnAnimation) ||
                !animatedEntity.HasAnimation(turningOffAnimation))
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
                IsOn = true;
                animatedEntity.PlayAnimation(onAnimation);
            }
            else if (animatedEntity.CurrentAnimation == turningOffAnimation)
            {
                IsOn = false;
                animatedEntity.PlayAnimation(offAnimation);
            }
        }
    }
}