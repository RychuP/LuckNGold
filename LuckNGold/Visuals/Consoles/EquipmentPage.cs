using LuckNGold.Config;
using LuckNGold.Visuals.Controls;
using LuckNGold.World.Monsters.Interfaces;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Object displayed in front of the <see cref="EquipmentPanel"/>.
/// Holds <see cref="CharacterLoadout"/> and any related information.
/// </summary>
internal class EquipmentPage : ScreenObject
{
    public EquipmentPage(IEquipment equipmentComponent)
    {
        int height = (GameSettings.CharacterWindowHeight - 4) * GameSettings.FontSize.Y;
        int width = (GameSettings.CharacterWindowWidth - 2) * GameSettings.FontSize.X;
        Position = (1 * GameSettings.FontSize.X, 3 * GameSettings.FontSize.Y);

        var charLoadout = new CharacterLoadout(equipmentComponent);
        int x = (width - charLoadout.Width) / 2;
        int y = (height - charLoadout.Height) / 2;
        charLoadout.Position = (x, y);
        Children.Add(charLoadout);

        var inventory = new Inventory();
        inventory.Position = (width - inventory.Width, 0);
        //Children.Add(inventory);
    }
}