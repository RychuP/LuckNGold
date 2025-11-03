using LuckNGold.Config;
using LuckNGold.Primitives;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Surface that represents slots from equipment or quick access windows.
/// </summary>
internal class SlotSurface : ScreenSurface
{
    readonly int _boxSize;
    readonly ScreenSurface _itemSurface;

    public SlotSurface(int boxSize) : base(boxSize, boxSize)
    {
        _boxSize = boxSize;

        // Draw slot border.
        var shapeParameters = ShapeParameters.CreateStyledBoxThin(Colors.SelectorBorder);
        var itemBorder = new Rectangle(0, 0, boxSize, boxSize);
        Surface.DrawBox(itemBorder, shapeParameters);

        // Create surface for the item.
        _itemSurface = new ScreenSurface(1, 1)
        {
            Font = Program.Font,
            UsePixelPositioning = true
        };

        // Specify the font size for the item.
        _itemSurface.FontSize *= boxSize - 2;

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
        int y = _boxSize - 1;
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
        _itemSurface.Surface.SetGlyph(0, 0, appearance.Glyph);
    }

    /// <summary>
    /// Clears the item surface.
    /// </summary>
    public void ClearItem()
    {
        _itemSurface.Surface.SetGlyph(0, 0, 0);
    }
}