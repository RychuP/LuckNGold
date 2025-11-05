using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Enums;
using SadConsole.UI;

namespace LuckNGold.Visuals.Windows;

internal class EquipmentWindow : ScreenSurface
{
    const int SlotSize = 7;
    const int SlotSpacing = 0;

    public EquipmentWindow() : base(SlotSize * 3 + SlotSpacing * 2, 
        SlotSize * 3 + SlotSpacing * 2)
    {
        AddSlot(BodyPart.Head, GetTranslatedPosition(1, 0), GetPlaceholder("Head"));
        AddSlot(BodyPart.Body, GetTranslatedPosition(1, 1), GetPlaceholder("Body"));
        AddSlot(BodyPart.Feet, GetTranslatedPosition(1, 2), GetPlaceholder("Feet"));
        AddSlot(BodyPart.LeftHand, GetTranslatedPosition(0, 1), GetPlaceholder("Weapon"));
        AddSlot(BodyPart.RightHand, GetTranslatedPosition(2, 1), GetPlaceholder("Shield"));
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

    void AddSlot(BodyPart bodyPart, Point position, ColoredGlyph placeHolder)
    {
        var slot = new Slot(SlotSize, $"{bodyPart}", placeHolder)
        {
            Position = position
        };
        Children.Add(slot);
    }
}