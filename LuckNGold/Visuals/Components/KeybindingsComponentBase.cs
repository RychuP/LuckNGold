using SadConsole.Input;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Components;

internal class KeybindingsComponentBase : KeybindingsComponent
{
    protected readonly static IEnumerable<(InputKey binding, Direction direction)> ViMotions =
    [
        ((InputKey)Keys.K, Direction.Up),
        (Keys.L, Direction.Right),
        (Keys.J, Direction.Down),
        (Keys.H, Direction.Left),
        (Keys.U, Direction.UpRight),
        (Keys.N, Direction.DownRight),
        (Keys.B, Direction.DownLeft),
        (Keys.Y, Direction.UpLeft)
    ];

    public KeybindingsComponentBase()
    {
        // Add motions.
        SetMotions(ViMotions);
        SetMotions(ArrowMotions);
        SetMotions(NumPadAllMotions);
        SetMotions(WasdMotions);
    }
}