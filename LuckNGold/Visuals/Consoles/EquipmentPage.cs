using LuckNGold.Config;
using LuckNGold.Visuals.Controls;
using LuckNGold.World.Monsters.Components.Interfaces;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Object displayed in front of the <see cref="EquipmentPanel"/>.
/// Holds <see cref="CharacterLoadout"/> and any related information.
/// </summary>
internal class EquipmentPage : ScreenObject
{
    readonly IEquipment _equipmentComponent;
    public CharacterLoadout CharacterLoadout { get; init; }

    public EquipmentPage(IEquipment equipmentComponent)
    {
        _equipmentComponent = equipmentComponent;

        int height = (GameSettings.CharacterWindowHeight - 4) * GameSettings.FontSize.Y;
        int width = (GameSettings.CharacterWindowWidth - 2) * GameSettings.FontSize.X;
        Position = (1 * GameSettings.FontSize.X, 3 * GameSettings.FontSize.Y);

        var charLoadout = new CharacterLoadout(equipmentComponent);
        int x = (width - charLoadout.Width) / 2;
        int y = (height - charLoadout.Height) / 2;
        charLoadout.Position = (x, y);
        Children.Add(charLoadout);
        CharacterLoadout = charLoadout;

        //var inventory = new Inventory();
        //inventory.Position = (width - inventory.Width, 0);
        //Children.Add(inventory);
    }

    public void Interact()
    {
        var selectedSlot = CharacterLoadout.SelectedSlot as EquipmentSlot;
        var equipSlot = selectedSlot!.EquipSlot;
        _equipmentComponent.Unequip(equipSlot);
    }
}