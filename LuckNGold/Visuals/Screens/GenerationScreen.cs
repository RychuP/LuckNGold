
using SadConsole.Instructions;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen shown during level generation.
/// </summary>
internal class GenerationScreen : ScreenSurface
{
    const int MaxDotCount = 5;
    readonly string _text = "Generating ";
    readonly TimeSpan _gap = TimeSpan.FromMilliseconds(500);
    string _dots = string.Empty;

    public GenerationScreen() : base(Program.Width, Program.Height)
    {
        int y = Height / 2;
        int maxTextWidth = _text.Length + MaxDotCount;
        int x = (Width - maxTextWidth) / 2;

        var instr = new InstructionSet() { RepeatCount = -1 }
            .Code(() =>
            {
                _dots += ".";
                Surface.Print(x, y, _text + _dots);
                if (_dots.Length > MaxDotCount)
                {
                    _dots = string.Empty;
                    Surface.Print(x, y, _text + new string(' ', MaxDotCount + 1));
                }
            })
            .Wait(_gap);
        SadComponents.Add(instr);
    }

    public void Reset()
    {
        _dots = string.Empty;
    }
}