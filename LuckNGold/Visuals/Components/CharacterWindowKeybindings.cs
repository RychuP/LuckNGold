using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using LuckNGold.Visuals.Windows;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Components;

internal class CharacterWindowKeybindings : GameScreenKeybindingsComponent
{
    CharacterWindow _characterWindow;

    public CharacterWindowKeybindings(GameScreen gameScreen, CharacterWindow characterWindow) : 
        base(gameScreen, gameScreen.Pointer)
    {
        _characterWindow = characterWindow;

        AddQuickAccessControls();
        AddCharacterWindowControls();
    }

    void AddCharacterWindowControls()
    {
        // Next tab keybinding.
        var nextTabInput = new InputKey(Keybindings.CharacterWindow, KeyModifiers.LeftShift);
        SetAction(nextTabInput, _characterWindow.SelectNextTab);


    }

    // Controls the pointer in the character window.
    protected override void MotionHandler(Direction direction)
    {
        _characterWindow.EquipmentPage.CharacterLoadout.SelectSlot(direction);
    }

    protected override void HandleEscape()
    {
        _characterWindow.IsVisible = false;
    }
}