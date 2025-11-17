using LuckNGold.Config;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Object that holds slots.
/// </summary>
internal class SlotHolder : ScreenObject
{
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
}