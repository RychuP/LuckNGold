using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.Visuals.Windows.Panels;

/// <summary>
/// Player equipment slots.
/// </summary>
/// <remarks>The <see cref="EquipmentSlots"/> class provides a visual interface for equipping 
/// items to different body parts. It initializes with predefined slots for head, body, feet, 
/// left hand, and right hand, each represented by a <see cref="Slot"/> object.</remarks>
internal class EquipmentSlots : ScreenSurface
{
    const int SlotSize = 7;
    const int SlotSpacing = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentSlots"/> class.
    /// </summary>
    public EquipmentSlots() : base(SlotSize * 3 + SlotSpacing * 2, 
        SlotSize * 3 + SlotSpacing * 2)
    {
        AddSlot(EquipSlot.Head, GetTranslatedPosition(1, 0), GetPlaceholder("Head"));
        AddSlot(EquipSlot.Body, GetTranslatedPosition(1, 1), GetPlaceholder("Torso"));
        AddSlot(EquipSlot.Feet, GetTranslatedPosition(1, 2), GetPlaceholder("Feet"));
        AddSlot(EquipSlot.LeftHand, GetTranslatedPosition(0, 1), GetPlaceholder("Weapon"));
        AddSlot(EquipSlot.RightHand, GetTranslatedPosition(2, 1), GetPlaceholder("Shield"));
    }

    public IEnumerable<Slot> Slots => Children
        .Cast<Slot>();

    static Point GetTranslatedPosition(int x, int y)
    {
        Point targetSize = (1, 1);
        Point sourceSize = (SlotSize + SlotSpacing, SlotSize + SlotSpacing);
        Point position = (x, y);
        return position.TranslateFont(sourceSize, targetSize);
    }

    static ColoredGlyph GetPlaceholder(string placeholderName)
    {
        var glyphDef = Program.Font.GetGlyphDefinition(placeholderName);
        return glyphDef.CreateColoredGlyph();
    }

    void AddSlot(EquipSlot equipSlot, Point position, ColoredGlyph placeHolder)
    {
        var slot = new Slot(SlotSize, $"{equipSlot}", placeHolder)
        {
            Position = position
        };
        Children.Add(slot);
    }
}