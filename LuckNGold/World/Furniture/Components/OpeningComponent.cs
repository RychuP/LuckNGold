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
internal class OpeningComponent : RogueLikeComponentBase<RogueLikeEntity>, IOpenable
{
    public event EventHandler? Opened;
    public event EventHandler? Closed;

    bool _isOpen;
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

    public OpeningComponent(bool isOpen = false) : base(false, false, false, false)
    {
        // assign the value quietly
        _isOpen = isOpen;
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

        // Open the opening and report success
        if (Parent is AnimatedRogueLikeEntity animated && animated.HasAnimation("Close"))
        {
            animated.PlayAnimation("Close");
            animated.AnimationChanged += (s, e) =>
            {
                IsOpen = false;
            };

            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false
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

        // Check for presence of any components that could prevent the entity being opened
        var lockable = Parent.AllComponents.GetFirstOrDefault<ILockable>();
        if (lockable is not null)
            return false;

        // Open the opening and report success
        if (Parent is AnimatedRogueLikeEntity animated && animated.HasAnimation("Open"))
        {
            animated.PlayAnimation("Open");
            animated.AnimationChanged += (s, e) =>
            {
                IsOpen = true;
            };

            // Operation was successful but the access will only be granted 
            // after the animation finished playing hence returning false
            return false;
        }

        // Grant access to entity and report success
        IsOpen = true;
        return true;
    }

    void OnOpened()
    {
        Opened?.Invoke(this, EventArgs.Empty);
    }

    public void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}