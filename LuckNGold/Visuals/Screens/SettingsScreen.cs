using LuckNGold.Visuals.Components;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

internal class SettingsScreen : MenuScreen
{
    public const string Name = "Settings";
    public const string Description = "Configure Gameplay and Keybindings";

    public SettingsScreen() : base()
    {
        PrintTitle(Name);

        Controls.Clear();
        AddButton(MotionsSelectorScreen.Name, Program.RootScreen.Show<MotionsSelectorScreen>, 
            MotionsSelectorScreen.Description);
        AddButton("Back", Program.RootScreen.ShowReturnScreen);

        FocusFirstControl();
    }
}