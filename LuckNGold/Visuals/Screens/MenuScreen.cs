using LuckNGold.Visuals.Components;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

abstract class MenuScreen : ControlsConsole, Screen
{
    /// <summary>
    /// Vertical spacing between controls.
    /// </summary>
    const int RowSpacing = 2;

    protected RootScreen RootScreen { get; init; }

    public MenuScreen(RootScreen rootScreen) : base(Program.Width, Program.Height)
    {
        RootScreen = rootScreen;
        SadComponents.Add(new MenuKeybindingsComponent());
    }

    /// <summary>
    /// Prints screen title.
    /// </summary>
    /// <param name="title">Title of the screen.</param>
    protected void PrintTitle(string title)
    {
        int x = (Width - title.Length) / 2;
        Surface.Print(x, 10, title, Color.Yellow);
    }

    /// <summary>
    /// Prints instructions to focused controls, if required.
    /// </summary>
    protected void PrintInstruction(string instruction = "")
    {
        if (string.IsNullOrEmpty(instruction))
        {
            instruction = " ".PadLeft(Width);
        }

        int x = (Width - instruction.Length) / 2;
        int y = GetFirstRow() - RowSpacing;
        Surface.Print(x, y, instruction);
    }

    protected void AddButton(string buttonText, Action buttonAction, string instruction = "")
    {
        var button = new Button(buttonText);
        button.Click += (o, e) => buttonAction();

        // Add instruction display.
        if (!string.IsNullOrEmpty(instruction))
        {
            button.Focused += (o, e) => PrintInstruction(instruction);
            button.Unfocused += (o, e) => PrintInstruction();
        }

        // Calculate position.
        int x = (Width - buttonText.Length - 4) / 2;
        int y = GetNextRow();
        button.Position = (x, y);

        // Add to controls.
        Controls.Add(button);
    }

    protected void AddCheckBox(string text, bool isSelected, EventHandler handler)
    {
        var checkBox = new CheckBox(text) { IsSelected = isSelected };
        checkBox.IsSelectedChanged += handler;

        // Add instruction display.
        checkBox.Focused += (o, e) => PrintInstruction("At least one set of motions " +
            "must be selected.");
        checkBox.Unfocused += (o, e) => PrintInstruction();

        // Calculate position.
        int x = (Width - text.Length - 3) / 2;
        int y = GetNextRow();
        checkBox.Position = (x, y);

        // Add to controls.
        Controls.Add(checkBox);
    }

    /// <summary>
    /// Gets the next row where a new control can be placed.
    /// </summary>
    int GetNextRow()
    {
        var lastControl = Controls.LastOrDefault();
        return lastControl != null ? lastControl.Position.Y + RowSpacing : GetFirstRow();
    }

    /// <summary>
    /// Gets the first row where the controls are initially placed.
    /// </summary>
    int GetFirstRow() =>
        (Height - 1) / 2;

    public void FocusFirstControl()
    {
        Controls.FocusedControl = Controls[0];
    }

    public void UpdateKeybindings(CheckBox checkBox)
    {
        if (SadComponents.Where(c => c is MenuKeybindingsComponent)
            .FirstOrDefault() is MenuKeybindingsComponent keybindingsComponent)
        {
            switch (checkBox.Text)
            {
                case SettingsScreen.ArrowButtonsText:
                    if (checkBox.IsSelected)
                        keybindingsComponent.AddArrowMotions();
                    else
                        keybindingsComponent.RemoveArrowMotions();
                    break;

                case SettingsScreen.NumpadButtonsText:
                    if (checkBox.IsSelected)
                        keybindingsComponent.AddNumpadMotions();
                    else
                        keybindingsComponent.RemovedNumpadMotions();
                    break;

                case SettingsScreen.WasdButtonsText:
                    if (checkBox.IsSelected)
                        keybindingsComponent.AddWasdMotions();
                    else
                        keybindingsComponent.RemoveWasdMotions();
                    break;

                case SettingsScreen.ViButtonsText:
                    if (checkBox.IsSelected)
                        keybindingsComponent.AddViMotions();
                    else
                        keybindingsComponent.RemoveViMotions();
                    break;
            }
        }
    }
}