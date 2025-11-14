namespace LuckNGold.Visuals.Windows.Panels;

internal class EquipmentPanel : CharacterWindowPanel
{
    public EquipmentPanel(int width, int height) : base(width, height)
    {
        Name = "Equipment Panel";

        var equipmentSlots = new EquipmentSlots();
        int x = (Width - equipmentSlots.Width) / 2;
        int y = (Height - equipmentSlots.Height) / 2;
        equipmentSlots.Position = (x, y);
        
    }
}