using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.Visuals.Consoles;

internal class EquipmentSlot : Slot
{
    ColoredGlyph _placeHolder;
    public EquipSlot EquipSlot { get; init; }

    public EquipmentSlot(int size, EquipSlot equipSlot, ColoredGlyph placeHolder) : base(size)
    {
        _placeHolder = placeHolder;
        EquipSlot = equipSlot;

        // Draw place holder.
        EraseItem();
    }

    public override void EraseItem()
    {
        _placeHolder.CopyAppearanceTo(ItemSurface.Surface[0]);
        ItemSurface.IsDirty = true;
    }
}