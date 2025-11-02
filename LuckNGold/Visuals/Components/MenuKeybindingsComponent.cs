
using LuckNGold.Visuals.Screens;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Components;

internal class MenuKeybindingsComponent() : KeybindingsComponentBase()
{
    protected override void MotionHandler(Direction direction)
    {
        if (Parent is not MenuScreen menuScreen) return;

        if (direction == Direction.Up)
            menuScreen.Controls.TabPreviousControl();
        else if (direction == Direction.Down)
            menuScreen.Controls.TabNextControl();
    }

    protected override void HandleEscape()
    {
        if (Parent is MainMenuScreen)
            RootScreen.Exit();
        else if (Parent is PauseScreen)
            Program.RootScreen.Show<GameScreen>();
        else if (Parent is SettingsScreen settingsScreen &&
            settingsScreen.GetReturnButton() is Button returnButton)
            returnButton.InvokeClick();
        else
            Program.RootScreen.ShowPrevScreen();
    }
}