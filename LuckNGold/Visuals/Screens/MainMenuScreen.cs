using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed when the game first starts or when player chooses 
/// to stop the game and go back to the main menu.
/// </summary>
internal class MainMenuScreen : ControlsConsole
{
    readonly RootScreen _root;

    public MainMenuScreen(RootScreen root) : base(Program.Width, Program.Height)
    {
        _root = root;

        string title = "-= Luck N' Gold =-";
        int x = (Width - title.Length) / 2;
        Surface.Print(x, 10, title, Color.Yellow);

        string buttonText = "Start Game";
        var startButton = new Button(buttonText);
        startButton.Click += (o, e) => _root.CreateNewGame();
        x = (Width - buttonText.Length - 4) / 2;
        int y = (Height - startButton.Surface.Height) / 2;
        startButton.Position = (x, y);
        Controls.Add(startButton);
    }
}