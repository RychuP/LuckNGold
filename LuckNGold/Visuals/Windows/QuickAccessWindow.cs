using LuckNGold.Config;
using LuckNGold.Primitives;
using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Window that displays contents of <see cref="QuickAccessComponent"/> attached to the player.
/// </summary>
internal class QuickAccessWindow : ScreenSurface
{
    // Size of the square that forms a border around an item when keyboard shortcut is pressed
    const int SelectorBoxSize = 5;

    // Parameters for the border of the selected item
    readonly ShapeParameters _shapeParameters;

    // Surface set to the graphical font used in the game that displays items from inventory
    readonly ScreenSurface _itemDisplay;

    public QuickAccessWindow(QuickAccessComponent quickAccess) : 
        base(QuickAccessComponent.Max * SelectorBoxSize +
        QuickAccessComponent.Max - 1, SelectorBoxSize)
    {
        _shapeParameters = ShapeParameters.CreateStyledBoxThin(Colors.SelectorBorder);

        // Subscribe to quick action events
        quickAccess.Changed += QuickAccess_OnSlotChanged;

        // Draw slots for the inventory items
        DrawSlots();

        // Create surface that will display the actual inventory items
        _itemDisplay = CreateItemSurface();
        Children.Add(_itemDisplay);
    }

    /// <summary>
    /// Draws borders around inventory slots and displays keyboard shortcut for each.
    /// </summary>
    void DrawSlots()
    {
        for (int i = 0; i < QuickAccessComponent.Max; i++)
        {
            int x = i * SelectorBoxSize + i;
            var itemBorder = new Rectangle(x, 0, SelectorBoxSize, SelectorBoxSize);
            Surface.DrawBox(itemBorder, _shapeParameters);
            int index = i < 9 ? i + 1 : 0;
            Surface.Print(x + 1, SelectorBoxSize - 1, $"{(char)180} {(char)195}");
            Surface.Print(x + 2, SelectorBoxSize - 1, $"{index}", Colors.SlotDigit);
        }
    }

    ScreenSurface CreateItemSurface()
    {
        var itemSurface = new ScreenSurface(QuickAccessComponent.Max * 2 - 1, 1)
        {
            Font = Program.Font,
            UsePixelPositioning = true
        };
        itemSurface.FontSize *= 3;
        int offset = (HeightPixels - itemSurface.HeightPixels) / 2;
        itemSurface.Position = AbsolutePosition + (offset, offset);
        return itemSurface;
    }

    /// <summary>
    /// Creates a highlight around the selected item.
    /// </summary>
    /// <param name="index">Index of the item in the inventory.</param>
    /// <exception cref="ArgumentOutOfRangeException">Exception throw when index is
    /// outside the bounds of the inventory.</exception>
    public void Select(int index)
    {
        //if (index < 0 || index >= QuickAccessComponent.MaxItemsCount ||
        //    index >= _inventory.Items.Count) 
        //    throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <summary>
    /// Displays the static appearance of the item with the given index
    /// in the appropriate slot of the window.
    /// </summary>
    void DisplayItem(RogueLikeEntity item, int index)
    {
        ColoredGlyphBase appearance = item is AnimatedRogueLikeEntity animated ?
            animated.StaticAppearance : item.AppearanceSingle!.Appearance;
        _itemDisplay.Surface.SetGlyph(index * 2, 0, appearance.Glyph);
    }

    void EraseItem(int index)
    {
        _itemDisplay.Surface.SetGlyph(index * 2, 0, 0);
    }

    void QuickAccess_OnSlotChanged(object? _, InventoryChangedEventArgs e)
    {
        if (e.Index < 0 || e.Index >= QuickAccessComponent.Max)
            throw new IndexOutOfRangeException(nameof(e.Index));

        if (e.NewItem is null)
            EraseItem(e.Index);
        else
            DisplayItem(e.NewItem, e.Index);
    }
}