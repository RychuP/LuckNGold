using LuckNGold.Config;
using LuckNGold.Primitives;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Surface that represents slots for equipment or items in player inventory.
/// </summary>
internal class Slot : ScreenSurface
{
    protected ScreenSurface ItemSurface { get; init; }

    bool _isSelected = false;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnIsSelectedChanged();
        }
    }

    public Slot(int size, Color? bgcolor = null) : base(size, size)
    {
        DrawBorder();

        // Create surface for the item.
        ItemSurface = new ScreenSurface(1, 1)
        {
            Font = Program.Font,
            UsePixelPositioning = true
        };
        Children.Add(ItemSurface);

        if (bgcolor != null)
        {
            ItemSurface.Surface.DefaultBackground = bgcolor.Value;
            ItemSurface.Surface.Clear();
        }

        // Specify the font size for the item.
        ItemSurface.FontSize *= size - 2;

        // Shift pixel position of the item surface to place it in the middle of the border.
        int offset = (HeightPixels - ItemSurface.HeightPixels) / 2;
        ItemSurface.Position = AbsolutePosition + (offset, offset);
    }

    /// <summary>
    /// Prints a digit on the bottom line of the border.
    /// </summary>
    /// <param name="digit">Digit to be displayer on the border.</param>
    public void PrintDigit(int digit)
    {
        int y = Width - 1;
        Surface.Print(1, y, $"{(char)180} {(char)195}");
        Surface.Print(2, y, $"{digit}", Theme.SlotDigit);
    }

    /// <summary>
    /// Shows a static appearance of the given item.
    /// </summary>
    /// <param name="item">Item to be shown.</param>
    public void ShowItem(RogueLikeEntity item)
    {
        ColoredGlyphBase appearance = item is AnimatedRogueLikeEntity animated ?
            animated.StaticAppearance : item.AppearanceSingle!.Appearance;
        ItemSurface.Surface[0].CopyAppearanceFrom(appearance);
        ItemSurface.Surface.IsDirty = true;
    }

    /// <summary>
    /// Clears the item surface.
    /// </summary>
    public virtual void EraseItem()
    {
        ItemSurface.Surface.SetGlyph(0, 0, 0);
    }

    void DrawBorder()
    {
        var borderColor = IsSelected ? Theme.SlotSelected : Theme.SlotBorder;
        var shapeParameters = ShapeParameters.CreateStyledBoxThin(borderColor);
        var itemBorder = new Rectangle(0, 0, Width, Height);
        Surface.DrawBox(itemBorder, shapeParameters);
    }

    void OnIsSelectedChanged()
    {
        DrawBorder();
    }
}