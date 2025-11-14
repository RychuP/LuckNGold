namespace LuckNGold.Visuals.Windows.Panels;

internal class EquipmentPanel : CharacterWindowPanel
{
    public EquipmentPanel(int width, int height) : base(width, height)
    {
        Name = "Equipment Panel";

        var slot = new CharacterWindowSlot(7);
        slot.Position = (1, 3);
        Controls.Add(slot);
    }
}