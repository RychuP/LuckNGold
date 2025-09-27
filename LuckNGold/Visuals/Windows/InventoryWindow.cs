using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

internal class InventoryWindow : ScreenSurface
{
    // Size of the square that forms a border around an item when keyboard shortcut is pressed
    const int SelectorBoxSize = 5;

    // Max number of items that can be displayed in the window
    const int MaxItemsCount = 10;

    // Inventory component that has its contents displayed in this window
    readonly InventoryComponent _inventory;

    // Cache of the inventory items for the purpose of not loosing indices when inventory changes
    readonly RogueLikeEntity?[] _items = new RogueLikeEntity[MaxItemsCount];

    // Parameters for the border of the selected item
    readonly ShapeParameters _shapeParameters;

    // Surface set to the graphical font used in the game that displays items from inventory
    readonly ScreenSurface _itemDisplay;

    public InventoryWindow(InventoryComponent inventory) : 
        base(MaxItemsCount * SelectorBoxSize + MaxItemsCount - 1, SelectorBoxSize)
    {
        _shapeParameters = ShapeParameters.CreateStyledBoxThin(Colors.SelectorBorder);
        UseKeyboard = true;

        // Subscribe to inventory events
        _inventory = inventory;
        _inventory.ItemAdded += Inventory_OnItemAdded;
        _inventory.ItemRemoved += Inventory_OnItemRemoved;

        // Draw slots for the inventory items
        DrawSlots();

        // Create surface that will display the actual inventory items
        _itemDisplay = CreateItemSurface();
        Children.Add(_itemDisplay);
    }

    public RogueLikeEntity? GetItem(int index) =>
        _items[index];

    /// <summary>
    /// Draws borders around inventory slots and displays keyboard shortcut for each.
    /// </summary>
    void DrawSlots()
    {
        for (int i = 0; i < MaxItemsCount; i++)
        {
            int x = i * SelectorBoxSize + i;
            var itemBorder = new Rectangle(x, 0, SelectorBoxSize, SelectorBoxSize);
            Surface.DrawBox(itemBorder, _shapeParameters);
            int index = i < 9 ? i + 1 : 0;
            Surface.Print(x + 1, SelectorBoxSize - 1, $"{(char)180} {(char)195}");
            Surface.Print(x + 2, SelectorBoxSize - 1, $"{index}", Colors.SelectorNumber);
        }
    }

    ScreenSurface CreateItemSurface()
    {
        var itemSurface = new ScreenSurface(MaxItemsCount * 2 - 1, 1)
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
        if (index < 0 || index >= MaxItemsCount || index >= _inventory.Items.Count) 
            throw new ArgumentOutOfRangeException(nameof(index));
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

    int GetNextEmptySlot()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] is null)
                return i;
        }
        return -1;
    }

    void Inventory_OnItemAdded(object? sender, InventoryItemEventArgs e)
    {
        int index = GetNextEmptySlot();
        if (index < 0)
            throw new InvalidOperationException("Item was added to the inventory " +
                "but all slots in the window are already taken.");
        _items[index] = e.Item;
        DisplayItem(e.Item, index);
    }

    void Inventory_OnItemRemoved(object? sender, InventoryItemEventArgs e)
    {
        int index = Array.IndexOf(_items, e.Item);
        if (index < 0)
            throw new InvalidOperationException("Item removed from the inventory " +
                "could not be found in the window.");
        _items[index] = null;
        EraseItem(index);
    }
}