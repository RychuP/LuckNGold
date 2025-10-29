namespace LuckNGold.Visuals.Screens;

internal class SettingsScreen : MenuScreen
{
    public SettingsScreen(RootScreen rootScreen) : base(rootScreen)
    {
        PrintTitle("-= Settings =-");
        DisplaySettingButtons();
    }

    void DisplaySettingButtons()
    {
        Controls.Clear();
        AddButton("Motion Controls", DisplayMotionControlSelectors);
        AddButton("Back", RootScreen.Show<MainMenuScreen>);
        FocusFirstControl();
    }

    void DisplayMotionControlSelectors()
    {
        Controls.Clear();
        AddButton("Back", DisplaySettingButtons);
        FocusFirstControl();
    }
}