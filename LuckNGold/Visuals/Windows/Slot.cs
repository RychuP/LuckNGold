using LuckNGold.Config;
using LuckNGold.Primitives;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Surface that represents slots for equipment or items in player inventory.
/// </summary>
internal class Slot : ScreenSurface
{
    readonly ScreenSurface _itemSurface;
    readonly ColoredGlyph? _placeHolder;
    public string Tag { get; init; }

    public Slot(int size, string tag, ColoredGlyph? placeHolder = null) : base(size, size)
    {
        _placeHolder = placeHolder;
        Tag = tag;

        // Draw slot border.
        var shapeParameters = ShapeParameters.CreateStyledBoxThin(Colors.SelectorBorder);
        var itemBorder = new Rectangle(0, 0, size, size);
        Surface.DrawBox(itemBorder, shapeParameters);

        // Create surface for the item.
        _itemSurface = new ScreenSurface(1, 1)
        {
            Font = Program.Font,
            UsePixelPositioning = true
        };
        Children.Add(_itemSurface);

        // Draw placeholder if any.
        EraseItem();

        // Specify the font size for the item.
        _itemSurface.FontSize *= size - 2;

        // Shift pixel position of the item surface to place it in the middle of the border.
        int offset = (HeightPixels - _itemSurface.HeightPixels) / 2;
        _itemSurface.Position = AbsolutePosition + (offset, offset);
    }

    /// <summary>
    /// Prints a digit on the bottom line of the border.
    /// </summary>
    /// <param name="digit">Digit to be displayer on the border.</param>
    public void PrintDigit(int digit)
    {
        int y = Width - 1;
        Surface.Print(1, y, $"{(char)180} {(char)195}");
        Surface.Print(2, y, $"{digit}", Colors.SlotDigit);
    }

    /// <summary>
    /// Shows a static appearance of the given item.
    /// </summary>
    /// <param name="item">Item to be shown.</param>
    public void ShowItem(RogueLikeEntity item)
    {
        ColoredGlyphBase appearance = item is AnimatedRogueLikeEntity animated ?
            animated.StaticAppearance : item.AppearanceSingle!.Appearance;
        appearance.CopyAppearanceTo(_itemSurface.Surface[0]);
        //_itemSurface.Surface.SetGlyph(0, 0, appearance.Glyph);
    }

    /// <summary>
    /// Clears the item surface.
    /// </summary>
    public void EraseItem()
    {
        if (_placeHolder != null)
            _placeHolder.CopyAppearanceTo(_itemSurface.Surface[0]);
        else
            _itemSurface.Surface.SetGlyph(0, 0, 0);
    }
}