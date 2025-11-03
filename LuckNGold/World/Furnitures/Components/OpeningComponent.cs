using LuckNGold.Primitives;
using LuckNGold.World.Furnitures.Enums;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that have some sort of an opening that first needs 
/// to be opened in order for the entity to become accessible.
/// </summary>
internal class OpeningComponent(string openAnimation = "", string closedAnimation = "",
    string openingAnimation = "", string closingAnimation = "", bool isOpen = false) 
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IOpenable
{
    public event EventHandler? Opened;
    public event EventHandler? Closed;
    public event EventHandler<ValueChangedEventArgs<bool>>? StateChanging;

    bool _stateChangeCanGoAhead = false;

    string _openAnimation = openAnimation;
    public string OpenAnimation
    {
        get => _openAnimation;
        set
        {
            if (_openAnimation == value) return;
            if (!IsValidAnimation(value))
                throw new InvalidOperationException("Parent entity is missing animation.");
            _openAnimation = value;
            OnOpenAnimationChanged();
        }
    }

    string _closedAnimation = closedAnimation;
    public string ClosedAnimation
    {
        get => _closedAnimation;
        set
        {
            if (_closedAnimation == value) return;
            if (!IsValidAnimation(value))
                throw new InvalidOperationException("Parent entity is missing animation.");
            _closedAnimation = value;
        }
    }

    string _openingAnimation = openingAnimation;
    public string OpeningAnimation
    {
        get => _openingAnimation;
        set
        {
            if (_openingAnimation == value) return;
            if (!IsValidAnimation(value))
                throw new InvalidOperationException("Parent entity is missing animation.");
            _openingAnimation = value;
        }
    }

    string _closingAnimation = closingAnimation;
    public string ClosingAnimation
    {
        get => _closingAnimation;
        set
        {
            if (_closingAnimation == value) return;
            if (!IsValidAnimation(value))
                throw new InvalidOperationException("Parent entity is missing animation.");
            _closingAnimation = value;
        }
    }

    bool _isOpen = isOpen;
    /// <summary>
    /// State of the opening.
    /// </summary>
    public bool IsOpen
    {
        get => _isOpen;
        private set
        {
            if (value ==  _isOpen) return;
            _isOpen = value;
            if (_isOpen) OnOpened();
            else OnClosed();
        }
    }

    /// <summary>
    /// Check intended to be made before closing remotely.
    /// </summary>
    /// <returns></returns>
    //public bool CanClose()
    //{
    //    if (Parent is null || Parent.CurrentMap is null)
    //        return false;

    //    // Opening needs to be open before it can be closed.
    //    if (!IsOpen)
    //        return false;

    //    // Check if lock component is present -> this should not happen in open state.
    //    if (Parent.AllComponents.GetFirstOrDefault<ILockable>() is not null)
    //        return false;

    //    // Check if actuator is present -> at the stage of the CanClose check,
    //    // the actuator should be extended, meaning the opening is in Open state.
    //    if (Parent.AllComponents.GetFirstOrDefault<IActuator>() is IActuator actuatorComponent
    //        && actuatorComponent.State != ActuatorState.Extended) 
    //        return false;

    //    // Checks specific to walkable parents.
    //    if (Parent.IsWalkable)
    //    {
    //        var map = Parent.CurrentMap;

    //        // Walkable parent will most likely want to change walkability to false when closed.
    //        if (map.GetEntitiesAt<RogueLikeEntity>(Parent.Position).Count() > 1)
    //            // Can't close if there is another entity on top of the parent.
    //            return;

    //        // Checks specific to double doors and gates.
    //        if (Parent.Name == "Door" || Parent.Name == "Gate")
    //        {
    //            var leftEntity = map.GetEntityAt<RogueLikeEntity>(Parent.Position + Direction.Left);
    //            var rightEntity = map.GetEntityAt<RogueLikeEntity>(Parent.Position + Direction.Right);

    //            if (leftEntity.
    //        }
    //    }
    //}

    public void Close()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        // Opening needs to be open before it can be closed.
        if (!IsOpen) return;

        // Announce intention to close.
        OnStateChanging(false);
        if (!_stateChangeCanGoAhead)
            return;

        // Check if lock component is present -> this should not happen in open state.
        if (Parent.AllComponents.GetFirstOrDefault<ILockable>() is not null)
            throw new InvalidOperationException("Locked open is not a valid state.");

        // Check if actuator is present -> can't close if not retracted.
        if (Parent.AllComponents.GetFirstOrDefault<IActuator>() is IActuator actuatorComponent
            && actuatorComponent.State != ActuatorState.Retracted) return;

        // Checks specific to walkable parents.
        //if (Parent.IsWalkable)
        //{
        //    var map = Parent.CurrentMap;

        //    // Walkable parent will most likely want to change walkability to false when closed.
        //    if (map.GetEntitiesAt<RogueLikeEntity>(Parent.Position).Count() > 1)
        //        // Can't close if there is another entity on top of the parent.
        //        return;

        //    // Checks specific to double doors and gates.
        //    if (Parent.Name == "Door" || Parent.Name == "Gate")
        //    {
        //        var leftEntity = map.GetEntityAt<RogueLikeEntity>(Parent.Position + Direction.Left);
        //        var rightEntity = map.GetEntityAt<RogueLikeEntity> (Parent.Position + Direction.Right);

        //        if (leftEntity.
        //    }
        //}

        // Check if parent is animated.
        if (Parent is AnimatedRogueLikeEntity animated)
        {
            // Refuse to act if an opening/closing animation is already playing.
            if (!animated.IsPlaying)
            {
                // Play closing animation.
                // Actual state change will happen in the animation changed event handler.
                animated.PlayAnimation(ClosingAnimation);
            }
        }
        else
        {
            // Not animated -> close immediately.
            IsOpen = false;
        }
    }

    public void Open()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        // Opening needs to be closed before it can be closed.
        if (IsOpen) return;

        // Announce intention to open.
        OnStateChanging(true);
        if (!_stateChangeCanGoAhead)
            return;

        // Check if lock component is present -> can't open until lock is removed.
        if (Parent.AllComponents.GetFirstOrDefault<ILockable>() is not null) return;

        // Check if actuator is present -> can't open if not extended.
        if (Parent.AllComponents.GetFirstOrDefault<IActuator>() is IActuator actuatorComponent
            && actuatorComponent.State != ActuatorState.Extended) return;

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animated)
        {
            // Refuse to act if an opening/closing animation is already playing.
            if (!animated.IsPlaying)
            {
                // Play opening animation.
                // Actual state change will happen in the animation changed event handler.
                animated.PlayAnimation(OpeningAnimation);
            }
        }
        else
        {
            // Not animated -> open immediately.
            IsOpen = true;
        }
    }

    public void Interact(RogueLikeEntity interactor)
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    bool IsValidAnimation(string animationName)
    {
        if (Parent is AnimatedRogueLikeEntity animatedEntity
            && !animatedEntity.HasAnimation(animationName))
            return false;
        return true;
    }

    public void StopStateChanging() =>
        _stateChangeCanGoAhead = false;

    void OnStateChanging(bool desiredOpenState)
    {
        _stateChangeCanGoAhead = true;
        var args = new ValueChangedEventArgs<bool>(!desiredOpenState, desiredOpenState);
        StateChanging?.Invoke(this, args);
    }

    void OnOpened()
    {
        Opened?.Invoke(this, EventArgs.Empty);

        if (Parent is AnimatedRogueLikeEntity animatedEntity
            && animatedEntity.AllComponents.GetFirstOrDefault<IActuator>()
            is IActuator actuatorComponent && actuatorComponent.State == ActuatorState.Retracted)
        {
            // There is a chance, if the entity is animated, that the actuator may have retracted,
            // but the animation playing prevented action.
            Close();
        }
    }

    void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);

        if (Parent is AnimatedRogueLikeEntity animatedEntity
            && animatedEntity.AllComponents.GetFirstOrDefault<IActuator>()
            is IActuator actuatorComponent && actuatorComponent.State == ActuatorState.Extended)
        {
            // There is a chance, if the entity is animated, that the actuator may have extended,
            // but the animation playing prevented action.
            Open();
        }
    }

    void OnOpenAnimationChanged()
    {
        if (Parent is AnimatedRogueLikeEntity animatedEntity)
        {
            animatedEntity.PlayAnimation(OpenAnimation);
            animatedEntity.DefaultAnimation = OpenAnimation;
        }
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (host is RogueLikeEntity entity)
        {
            if (entity is AnimatedRogueLikeEntity animatedEntity)
            {
                if (!animatedEntity.HasAnimation(OpenAnimation) ||
                    !animatedEntity.HasAnimation(ClosedAnimation) ||
                    !animatedEntity.HasAnimation(OpeningAnimation) ||
                    !animatedEntity.HasAnimation(ClosingAnimation))
                    throw new InvalidOperationException("Host is missing animations.");

                animatedEntity.Finished += AnimatedRogueLikeEntity_OnFinished;
            }

            if (entity.AllComponents.GetFirstOrDefault<IActuator>() is
                IActuator actuator)
            {
                actuator.StateChanged += Actuator_OnStateChanged;
            }
        }
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);

        if (host is RogueLikeEntity entity)
        {
            if (entity is AnimatedRogueLikeEntity animatedEntity)
            {
                animatedEntity.Finished -= AnimatedRogueLikeEntity_OnFinished;
            }

            if (entity.AllComponents.GetFirstOrDefault<IActuator>() is
                IActuator actuator)
            {
                actuator.StateChanged -= Actuator_OnStateChanged;
            }
        }
    }

    void Actuator_OnStateChanged(object? o, EventArgs e)
    {
        if (o is IActuator actuator)
        {
            if (actuator.State == ActuatorState.Extended)
                Open();
            else
                Close();
        }
    }

    void AnimatedRogueLikeEntity_OnFinished(object? o, EventArgs e)
    {
        if (o is AnimatedRogueLikeEntity animatedEntity)
        {
            if (animatedEntity.CurrentAnimation == OpeningAnimation)
            {
                IsOpen = true;
                animatedEntity.PlayAnimation(OpenAnimation);
                animatedEntity.DefaultAnimation = OpenAnimation;
            }
            else if (animatedEntity.CurrentAnimation == ClosingAnimation)
            {
                IsOpen = false;
                animatedEntity.PlayAnimation(ClosedAnimation);
                animatedEntity.DefaultAnimation = ClosedAnimation;
            }
        }
    }
}