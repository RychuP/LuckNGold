using LuckNGold.Config;
using SadConsole.Input;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays various information as part of the <see cref="StatusSurface"/>.
/// </summary>
abstract class InfoBox : ScreenSurface
{
    readonly Rectangle _clearBox;
    public Color TextColor = Color.White;
    readonly BoxDescription _description;

    public InfoBox(string header, string description) : base(header.Length, 2)
    {
        _clearBox = new(0, 1, Width, 1);
        Surface.DefaultBackground = Color.Black;
        Surface.Clear();
        Surface.Print(0, 0, header, Color.LightGreen);

        _description = new BoxDescription(description);
        int x = (Width - _description.Width) / 2;
        _description.Position = (x, Height + 1);
        Children.Add(_description);
    }

    /// <summary>
    /// Prints given text below the header.
    /// </summary>
    /// <param name="text">Text to be printed below the header.</param>
    public void Print(string text)
    {
        if (text.Length > Width)
            text = text[..Width];

        Surface.Clear(_clearBox);
        int x = (Width - text.Length) / 2;
        Surface.Print(x, 1, text, TextColor);
    }

    /// <summary>
    /// Prints given number below the header.
    /// </summary>
    /// <param name="number">Number to be printed below the header.</param>
    public void Print(int number) =>
        Print(number.ToString());

    protected override void OnMouseEnter(MouseScreenObjectState state)
    {
        _description.Show();
        base.OnMouseEnter(state);
    }

    protected override void OnMouseExit(MouseScreenObjectState state)
    {
        _description.Hide();
        base.OnMouseExit(state);
    }
}