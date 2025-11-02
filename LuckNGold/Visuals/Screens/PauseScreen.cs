namespace LuckNGold.Visuals.Screens;

internal class PauseScreen : MenuScreen
{
    public PauseScreen() : base()
    {
        Name = "Paused";
        PrintTitle(Name);
        AddButton("Continue", Program.RootScreen.Show<GameScreen>, "Return to Current Game");
        AddButton("Settings", Program.RootScreen.Show<SettingsScreen>,
            "Configure Gameplay and Keybindings");
        AddButton("Exit", Program.RootScreen.Show<MainMenuScreen>, "Return to Main Menu Page");
    }
}