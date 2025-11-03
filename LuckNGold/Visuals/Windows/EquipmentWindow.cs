using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Enums;
using SadConsole.UI;

namespace LuckNGold.Visuals.Windows;

internal class EquipmentWindow : ScreenSurface
{
    readonly Point _slotSpacing = (1, 1);

    readonly Dictionary<BodyPart, SlotSurface> _equipmentSlots;
    readonly CharacterPreviewSurface _characterPreviewSurface;

    public EquipmentWindow() : base(30, 20)
    {
        var player = MonsterFactory.Player();
        _characterPreviewSurface = new(player);

        int slotCount = Enum.GetValues(typeof(BodyPart)).Length;
        _equipmentSlots = new(slotCount);
    }
}