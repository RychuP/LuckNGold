using LuckNGold.Config;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed when the game first starts or when player chooses 
/// to stop the game and go back to the main menu.
/// </summary>
internal class MainMenuScreen : MenuScreen
{
    public const string Name = "Main Menu";

    public MainMenuScreen() : base()
    {
        PrintTitle(GameSettings.Title);
        AddButton("Start Game", Program.RootScreen.CreateNewGame, "Create a New Game");
        AddButton(SettingsScreen.Name, Program.RootScreen.Show<SettingsScreen>, 
            SettingsScreen.Description);
        AddButton("Exit", RootScreen.Exit);
    }
}