using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Windows;
using System;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    /// <summary>
    /// Window that deals with player character management.
    /// </summary>
    readonly CharacterWindow _characterWindow;

    public void ToggleCharacterWindow()
    {
        if (_characterWindow.IsVisible)
            _characterWindow.Hide();
        else
            _characterWindow.Show();
    }

    void CharacterWindow_OnIsVisibleChanged(object? o, EventArgs e)
    {
        IsFocused = !_characterWindow.IsVisible;
    }
}