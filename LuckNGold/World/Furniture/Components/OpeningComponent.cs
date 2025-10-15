using LuckNGold.Visuals;
using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that have some sort of opening that first needs to be
/// opened in order for the entity to become accessible.
/// </summary>
internal class OpeningComponent(string openingAnimation = "",
    string closingAnimation = "", bool isOpen = false) 
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
        set
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

        // Check for presence of componets that prevent opening
        var lockable = Parent.AllComponents.GetFirstOrDefault<ILockable>();

        // Can something be locked open? Someone is trying to close it 
        // while being locked open? Probably not a valid operation.
        if (lockable is not null)
            throw new InvalidOperationException("Entity is locked open. This is not a valid state.");

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animated && animated.HasAnimation(closingAnimation))
        {
            // Play closing animation. Actual closing will be handled by animation event handler.
            if (!animated.IsPlaying)
                animated.PlayAnimation(closingAnimation);

            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false
            return false;
        }

        // Close the opening and report success
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

        // Check for presence of any components that could prevent the entity being opened
        var lockable = Parent.AllComponents.GetFirstOrDefault<ILockable>();
        if (lockable is not null)
            return false;

        // Check the parent is animated
        if (Parent is AnimatedRogueLikeEntity animated && animated.HasAnimation(openingAnimation))
        {
            // Play opening animation. Actual opening will be handled by animation event handler.
            if (!animated.IsPlaying)
                animated.PlayAnimation(openingAnimation);
            
            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false
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

        // Anything static and big must be present on the map to be interactable
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
    }

    public void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    void AnimatedRogueLikeEntity_OnAnimationChanged(object? o, ValueChangedEventArgs<string> e)
    {
        if (IsOpen && e.OldValue == closingAnimation)
            IsOpen = false;
        else if (!IsOpen && e.OldValue == openingAnimation)
            IsOpen = true;
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (host is AnimatedRogueLikeEntity animatedEntity 
            && animatedEntity.HasAnimation(openingAnimation)
            && animatedEntity.HasAnimation(closingAnimation))
        {
            animatedEntity.AnimationChanged += AnimatedRogueLikeEntity_OnAnimationChanged;
        }
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);

        if (host is AnimatedRogueLikeEntity animatedEntity
            && animatedEntity.HasAnimation(openingAnimation)
            && animatedEntity.HasAnimation(closingAnimation))
        {
            animatedEntity.AnimationChanged -= AnimatedRogueLikeEntity_OnAnimationChanged;
        }
    }
}