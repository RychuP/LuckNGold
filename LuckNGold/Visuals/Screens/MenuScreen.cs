using LuckNGold.Visuals.Components;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Base class to all menu screens.
/// </summary>
abstract class MenuScreen() : ControlsConsole(Program.Width, Program.Height)
{
    /// <summary>
    /// Vertical spacing between controls.
    /// </summary>
    const int RowSpacing = 2;

    const int TitleRow = 10;

    /// <summary>
    /// Name of the <see cref="MenuScreen"/>.
    /// </summary>
    public string Name { get; protected set; } = "";

    /// <summary>
    /// Description of the page to be displayed as button instruction.
    /// </summary>
    public string Description { get; protected set; } = "";

    /// <summary>
    /// Text for the button leading back to this screen.
    /// </summary>
    public string BackButtonInstruction => $"Return to {Name} Page";

    public static readonly MenuKeybindingsComponent KeybindingsComponent = new();

    /// <summary>
    /// Gets an Exit or Back button, whichever is present.
    /// </summary>
    public Button? GetReturnButton() => Controls
        .Where(c => c is Button button && (button.Text == "Back" || button.Text == "Exit"))
        .FirstOrDefault() as Button;

    /// <summary>
    /// Prints screen title.
    /// </summary>
    /// <param name="title">Title of the screen.</param>
    protected void PrintTitle(string title, Color? color = null)
    {
        title = $"-= {title} =-";
        int x = (Width - title.Length) / 2;
        Surface.Print(x, TitleRow, title, color ?? Color.Yellow);
    }

    protected void EraseTitle()
    {
        var title = " ".PadLeft(Width);
        Surface.Print(0, TitleRow, title);
    }

    /// <summary>
    /// Prints instructions to focused controls, if required.
    /// </summary>
    protected void PrintInstruction(string bottomInstruction, string topInstruction = "")
    {
        int y = GetFirstRow() - RowSpacing * 2;
        Print(bottomInstruction);

        if (!string.IsNullOrEmpty(topInstruction))
        {
            y -= RowSpacing;
            Print(topInstruction);
        }

        void Print(string text)
        {
            int x = (Width - text.Length) / 2;
            Surface.Print(x, y, text, Color.DarkSeaGreen);
        }
    }

    protected void EraseInstructions()
    {
        var text = " ".PadLeft(Width);
        PrintInstruction(text, text);
    }

    protected void AddButton(string buttonText, Action buttonAction, 
        string bottomInstruction, string topInstruction = "")
    {
        var button = new Button(buttonText);
        button.Click += (o, e) => buttonAction();
        AddControl(button, buttonText.Length + 4, bottomInstruction, topInstruction);
    }

    protected void AddCheckBox(string text, bool isSelected, EventHandler handler,
        string bottomInstruction, string topInstruction = "")
    {
        var checkBox = new CheckBox(text) { IsSelected = isSelected };
        checkBox.IsSelectedChanged += handler;
        AddControl(checkBox, text.Length + 3, bottomInstruction, topInstruction);
    }

    void AddControl(ControlBase control, int controlWidth,
        string bottomInstruction, string topInstruction)
    {
        // Add instruction display.
        control.Focused += (o, e) => PrintInstruction(bottomInstruction, topInstruction);
        control.Unfocused += (o, e) => EraseInstructions();

        // Calculate position.
        int x = (Width - controlWidth) / 2;
        int y = GetNextRow();
        control.Position = (x, y);

        // Add to controls.
        Controls.Add(control);
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
}