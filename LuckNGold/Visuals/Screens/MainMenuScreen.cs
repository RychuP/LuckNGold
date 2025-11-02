namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed when the game first starts or when player chooses 
/// to stop the game and go back to the main menu.
/// </summary>
internal class MainMenuScreen : MenuScreen
{
    public MainMenuScreen() : base()
    {
        Name = "Main Menu";
        PrintTitle("Luck N' Gold");
        AddButton("Start Game", Program.RootScreen.CreateNewGame, "Create a New Game");
        AddButton("Settings", Program.RootScreen.Show<SettingsScreen>, 
            "Configure Gameplay and Keybindings");
        AddButton("Exit", RootScreen.Exit, $"Exit to {Environment.OSVersion}");
    }
}