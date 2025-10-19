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

        // Check for presence of componets that prevent opening
        var lockable = Parent.AllComponents.GetFirstOrDefault<ILockable>();

        // Can something be locked open? Someone is trying to close it 
        // while being locked open? Probably not a valid operation.
        if (lockable is not null)
            throw new InvalidOperationException("Entity is locked open. This is not a valid state.");

        // Check the parent is animated
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

        // Check for presence of any components that could prevent the entity being opened.
        var lockable = Parent.AllComponents.GetFirstOrDefault<ILockable>();
        if (lockable is not null)
            return false;

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
    }

    public void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
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