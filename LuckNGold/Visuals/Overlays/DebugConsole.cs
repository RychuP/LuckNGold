using LuckNGold.Config;

namespace LuckNGold.Visuals.Overlays;

/// <summary>
/// Overlay screen that serves as a debug console.
/// It can print various debug information.
/// </summary>
internal class DebugConsole : Console
{
    public static readonly DebugConsole Instance;

    static DebugConsole()
    {
        Instance = new DebugConsole();
        Print("Debug Console:");
    }

    private DebugConsole() : base(GameSettings.Width, GameSettings.Height - 9)
    {
        Position = (0, 4);
        IsVisible = false;
    }

    public static void Clear()
    {
        Instance.Clear();
        Print("Debug Console:");
    }

    public static void Print(string text)
    {
        if (GameSettings.DebugEnabled)
        {
            Instance.Cursor
            .Print(text)
            .NewLine();
        }
    }
}