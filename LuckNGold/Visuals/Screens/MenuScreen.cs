using LuckNGold.Visuals.Components;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

abstract class MenuScreen : ControlsConsole
{
    protected RootScreen RootScreen { get; init; }

    public MenuScreen(RootScreen rootScreen) : base(Program.Width, Program.Height)
    {
        RootScreen = rootScreen;
        SadComponents.Add(new MenuKeybindingsComponent());
    }

    protected void PrintTitle(string title)
    {
        int x = (Width - title.Length) / 2;
        Surface.Print(x, 10, title, Color.Yellow);
    }

    protected void AddButton(string buttonText, Action buttonAction)
    {
        var button = new Button(buttonText);
        button.Click += (o, e) => buttonAction();
        var lastControl = Controls.LastOrDefault();
        int x = (Width - buttonText.Length - 4) / 2;
        int y = lastControl != null ? lastControl.Position.Y + 2 : 
            (Height - button.Surface.Height) / 2;
        button.Position = (x, y);
        Controls.Add(button);
    }

    public void FocusFirstControl()
    {
        Controls.FocusedControl = Controls[0];
    }
}