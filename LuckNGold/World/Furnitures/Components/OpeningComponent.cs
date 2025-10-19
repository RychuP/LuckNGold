using LuckNGold.Visuals;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that have some sort of opening that first needs to be
/// opened in order for the entity to become accessible.
/// </summary>
internal class OpeningComponent(string openAnimation = "", string closedAnimation = "",
    string openingAnimation = "", string closingAnimation = "", bool isOpen = false) 
    : RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IOpenable
{
    public event EventHandler? Opened;
    public event EventHandler? Closed;

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

    public bool Close()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture or below has to be on the map.");

        if (!IsOpen)
            return true;

        // Check if lock component is present -> can only open once lock is removed.
        if (Parent.AllComponents.GetFirstOrDefault<ILockable>() is not null)
            return false;

        // Check if signal receiver is present -> can only open remotely.
        if (Parent.AllComponents.GetFirstOrDefault<ISignalReceiver>() is
            ISignalReceiver signalReceiver)
        {
            if (!signalReceiver.HasUnconsumedSignal)
                return false;
        }

        // Check if parent is animated.
        if (Parent is AnimatedRogueLikeEntity animated)
        {
            // Refuse to act if an opening/closing animation is already playing.
            if (!animated.IsPlaying)
            {
                // Play closing animation.
                // Actual state change will happen in the animation changed event handler.
                animated.PlayAnimation(closingAnimation);
            }

            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false.
            return false;
        }

        // Close the opening and report success.
        IsOpen = false;
        return true;
    }

    public bool Open()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture or below has to be on the map.");

        if (IsOpen)
            return true;

        // Check if lock component is present -> this should not happen in open state.
        if (Parent.AllComponents.GetFirstOrDefault<ILockable>() is not null)
            throw new InvalidOperationException("Locked open is not a valid state.");

        // Check if signal receiver is present -> can only close remotely.
        if (Parent.AllComponents.GetFirstOrDefault<ISignalReceiver>() is
            ISignalReceiver signalReceiver)
        {
            if (!signalReceiver.HasUnconsumedSignal)
                return false;
        }

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animated)
        {
            // Refuse to act if an opening/closing animation is already playing.
            if (!animated.IsPlaying)
            {
                // Play opening animation.
                // Actual state change will happen in the animation changed event handler.
                animated.PlayAnimation(openingAnimation);
            }
            
            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false.
            return false;
        }

        // Grant access to entity and report success
        IsOpen = true;
        return true;
    }

    public bool Interact(RogueLikeEntity interactor)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture and below needs to be on the map.");

        if (IsOpen)
        {
            return Close();
        }
        else
        {
            return Open();
        }
    }

    void OnOpened()
    {
        Opened?.Invoke(this, EventArgs.Empty);

        if (Parent!.AllComponents.GetFirstOrDefault<ISignalReceiver>() is
            ISignalReceiver signalReceiver && signalReceiver.HasUnconsumedSignal)
            signalReceiver.ConsumeSignal();
    }

    public void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);

        if (Parent!.AllComponents.GetFirstOrDefault<ISignalReceiver>() is
            ISignalReceiver signalReceiver && signalReceiver.HasUnconsumedSignal)
            signalReceiver.ConsumeSignal();
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (host is AnimatedRogueLikeEntity animatedEntity)
        {
            if (!animatedEntity.HasAnimation(openAnimation) ||
                !animatedEntity.HasAnimation(closedAnimation) ||
                !animatedEntity.HasAnimation(openingAnimation) ||
                !animatedEntity.HasAnimation(closingAnimation))
                throw new InvalidOperationException("Host is missing switch animations.");

            animatedEntity.Finished += AnimatedRogueLikeEntity_OnFinished;
        }

        if (host is RogueLikeEntity entity &&
            entity.AllComponents.GetFirstOrDefault<SignalReceiverComponent>() is 
            SignalReceiverComponent signalReceiver)
        {
            signalReceiver.SignalReceived += SignalReceiver_OnSignalReceived;
        }
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);

        if (host is AnimatedRogueLikeEntity animatedEntity)
        {
            animatedEntity.Finished -= AnimatedRogueLikeEntity_OnFinished;
        }

        if (host is RogueLikeEntity entity &&
            entity.AllComponents.GetFirstOrDefault<SignalReceiverComponent>() is
            SignalReceiverComponent signalReceiver)
        {
            signalReceiver.SignalReceived -= SignalReceiver_OnSignalReceived;
        }
    }

    void SignalReceiver_OnSignalReceived(object? o, EventArgs e)
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    void AnimatedRogueLikeEntity_OnFinished(object? o, EventArgs e)
    {
        if (o is AnimatedRogueLikeEntity animatedEntity)
        {
            if (animatedEntity.CurrentAnimation == openingAnimation)
            {
                IsOpen = true;
                animatedEntity.PlayAnimation(openAnimation);
            }
            else if (animatedEntity.CurrentAnimation == closingAnimation)
            {
                IsOpen = false;
                animatedEntity.PlayAnimation(closedAnimation);
            }
        }
    }
}