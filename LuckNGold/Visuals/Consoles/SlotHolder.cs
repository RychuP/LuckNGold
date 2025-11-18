using LuckNGold.Config;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Object that holds slots.
/// </summary>
internal class SlotHolder : ScreenObject
{
    /// <summary>
    /// Slot currently marked as selected.
    /// </summary>
    public Slot? SelectedSlot { get; private set; } = null;

    /// <summary>
    /// Size of an individual slot in cells.
    /// </summary>
    public int SlotSize { get; init; }

    /// <summary>
    /// Spacing between slots.
    /// </summary>
    public int SlotSpacing { get; init; }

    /// <summary>
    /// Number of slot columns.
    /// </summary>
    public int SlotCountX { get; init; }

    /// <summary>
    /// Number of slot rows.
    /// </summary>
    public int SlotCountY { get; init; }

    /// <summary>
    /// Width of the <see cref="SlotHolder"/> in pixels.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Height of the <see cref="SlotHolder"/> in pixels.
    /// </summary>
    public int Height { get; init; }

    public SlotHolder(int slotSize, int slotCountX, int slotCountY, int slotSpacing = 0)
    {
        SlotSize = slotSize;
        SlotSpacing = slotSpacing;
        SlotCountX = slotCountX;
        SlotCountY = slotCountY;

        Width = (slotSize * slotCountX + slotSpacing * (slotCountX - 1)) * GameSettings.FontSize.X;
        Height = (slotSize * slotCountY + slotSpacing * (SlotCountY - 1)) * GameSettings.FontSize.Y;
    }

    /// <summary>
    /// Translates a position from a slot sized 1x1 to the actual size of the slot.
    /// </summary>
    protected Point GetTranslatedPosition(int x, int y)
    {
        Point targetSize = (1, 1);
        Point sourceSize = (SlotSize + SlotSpacing, SlotSize + SlotSpacing);
        Point position = (x, y);
        return position.TranslateFont(sourceSize, targetSize);
    }

    protected Point GetTranslatedPosition(Point position) =>
        GetTranslatedPosition(position.X, position.Y);

    protected Point GetNormalizedPosition(int x, int y)
    {
        Point targetSize = (SlotSize + SlotSpacing, SlotSize + SlotSpacing);
        Point sourceSize = (1, 1);
        Point position = (x, y);
        return position.TranslateFont(sourceSize, targetSize);
    }

    protected Point GetNormalizedPosition(Point position) =>
        GetNormalizedPosition(position.X, position.Y);

    /// <summary>
    /// Sets position in cells.
    /// </summary>
    public void SetCellPosition(int x, int y)
    {
        Position = (x * GameSettings.FontSize.X, y * GameSettings.FontSize.Y);
    }

    /// <summary>
    /// Returns position in cells.
    /// </summary>
    public Point GetCellPosition()
    {
        int x = Position.X / GameSettings.FontSize.X;
        int y = Position.Y / GameSettings.FontSize.Y;
        return new Point(x, y);
    }

    /// <summary>
    /// Marks slot with the given index as selected.
    /// </summary>
    /// <param name="index">Index of the slot child.</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public void SelectSlot(int index)
    {
        if (index < 0 || index >= Children.Count)
            throw new IndexOutOfRangeException();

        if (SelectedSlot != null)
            SelectedSlot.IsSelected = false;

        if (Children[index] is Slot slot)
        {
            slot.IsSelected = true;
            SelectedSlot = slot;
        }
    }

    public void SelectSlot(Direction direction)
    {
        if (SelectedSlot is null)
        {
            SelectSlot(0);
        }
        else
        {
            var normalizedPosition = GetNormalizedPosition(SelectedSlot.Position);
            var normalizedTargetPosition = normalizedPosition + direction;
            var targetPosition = GetTranslatedPosition(normalizedTargetPosition);
            var targetSlot = Children
                .Where(c => c.Position == targetPosition)
                .Cast<Slot>()
                .FirstOrDefault();

            if (targetSlot != null)
            {
                SelectedSlot.IsSelected = false;
                targetSlot.IsSelected = true;
                SelectedSlot = targetSlot;
            }
        }
    }
}