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
        if (Keybindings.ViMotionsEnabled)
            AddViMotions();

        if (Keybindings.ArrowMotionsEnabled)
            AddArrowMotions();

        if (Keybindings.NumpadMotionsEnabled)
            AddNumpadMotions();

        if (Keybindings.WasdMotionsEnabled)
            AddWasdMotions();
    }

    public void AddViMotions() =>
        SetMotions(ViMotions);

    public void AddArrowMotions() =>
        SetMotions(ArrowMotions);

    public void AddNumpadMotions() =>
        SetMotions(NumPadAllMotions);

    public void AddWasdMotions() =>
        SetMotions(WasdMotions);

    public void RemoveViMotions() =>
        RemoveMotions(ViMotions.Select(e => e.binding));

    public void RemoveArrowMotions() =>
        RemoveMotions(ArrowMotions.Select(e => e.binding));

    public void RemovedNumpadMotions() =>
        RemoveMotions(NumPadAllMotions.Select(e => e.binding));
    
    public void RemoveWasdMotions() =>
        RemoveMotions(WasdMotions.Select(e => e.binding));
}