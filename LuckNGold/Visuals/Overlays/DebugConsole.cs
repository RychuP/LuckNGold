namespace LuckNGold.Visuals.Overlays;

/// <summary>
/// Overlay screen that serves as a debug console.
/// It can print various debug information.
/// </summary>
internal class DebugConsole : Console
{
    public DebugConsole() : base(Program.Width, Program.Height)
    {
        IsVisible = false;
    }
}