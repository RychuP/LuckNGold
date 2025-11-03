namespace LuckNGold.Visuals.Screens;

internal class PauseScreen : MenuScreen
{
    public const string Name = "Pause";

    public PauseScreen() : base()
    {
        PrintTitle(Name);
        AddButton("Continue", Program.RootScreen.Show<GameScreen>, "Return to Current Game");
        AddButton(SettingsScreen.Name , Program.RootScreen.Show<SettingsScreen>,
            SettingsScreen.Description);
        AddButton("Exit", Program.RootScreen.Show<MainMenuScreen>);
    }
}