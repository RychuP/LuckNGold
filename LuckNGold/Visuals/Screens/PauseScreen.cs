using SadConsole.UI;

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
        AddButton("Exit", PromptConfirm);
    }

    static void PromptConfirm()
    {
        Window.Prompt("Are you sure you want to quit current game?", "Yes", "No", CheckResponse);
    }

    static void CheckResponse(bool response)
    {
        if (response)
            Program.RootScreen.Show<MainMenuScreen>();
    }
}