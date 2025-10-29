namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed when the game first starts or when player chooses 
/// to stop the game and go back to the main menu.
/// </summary>
internal class MainMenuScreen : MenuScreen
{
    public MainMenuScreen(RootScreen root) : base(root)
    {
        // Create title.
        PrintTitle("-= Luck N' Gold =-");

        // Create buttons.
        AddButton("Start Game", RootScreen.CreateNewGame);
        AddButton("Settings", RootScreen.Show<SettingsScreen>);
        AddButton("Exit", RootScreen.Exit);
    }
}