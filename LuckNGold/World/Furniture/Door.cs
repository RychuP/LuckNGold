using SadRogue.Integration;

namespace LuckNGold.World.Furniture;

internal class Door(Point position, ColoredGlyph closed, ColoredGlyph opened, bool transparent = false)
    : RogueLikeEntity(position, closed, false, transparent, (int)GameMap.Layer.Furniture)
{
    public event EventHandler? Opened;
    public event EventHandler? Closed;

    readonly ColoredGlyph _openedAppearance = opened;
    readonly ColoredGlyph _closedAppearance = closed;

    /// <summary>
    /// Forms the second half of another door if set to true.
    /// </summary>
    public bool IsDouble { get; private set; } = false;

    /// <summary>
    /// First half of the double door system that controls this door.
    /// </summary>
    public Door? ParentDoor { get; private set; } = null;

    /// <summary>
    /// Whether the door is open or closed.
    /// </summary>
    bool _isOpen = false;
    public bool IsOpen
    {
        get => _isOpen;
        private set
        {
            if (value ==  _isOpen) return;
            _isOpen = value;
            OnIsOpenChanged(value);
        }
    }

    public void MakeDouble(Door parentDoor)
    {
        IsDouble = true;
        ParentDoor = parentDoor;
        ParentDoor.Opened += Door_OnOpened;
        ParentDoor.Closed += Door_OnClosed;
    }

    public void Open()
    {
        if (IsDouble)
            ParentDoor?.Open();
        else
            IsOpen = true;
    }

    public void Close()
    {
        if (IsDouble)
            ParentDoor?.Close();
        else
            IsOpen = false;
    }

    void OnIsOpenChanged(bool newState)
    {
        // change appearance
        var appearance = newState ? _openedAppearance : _closedAppearance;
        appearance.CopyAppearanceTo(AppearanceSingle!.Appearance, false);

        // trigger event
        if (newState) 
            Opened?.Invoke(this, EventArgs.Empty);
        else
            Closed?.Invoke(this, EventArgs.Empty);
    }

    // This event handler should only be attached to double doors.
    void Door_OnOpened(object? sender, EventArgs e)
    {
        IsOpen = true;
    }

    // This event handler should only be attached to double doors.
    void Door_OnClosed(object? sender, EventArgs e)
    {
        IsOpen = false;
    }
}