using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using SadConsole.Input;
using SadConsole.UI.Controls;
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
        AddMotions();
        AddEscapeHandling();
    }

    void AddMotions()
    {
        if (Keybindings.ViMotionsEnabled)
            AddViMotions();

        if (Keybindings.ArrowMotionsEnabled)
            AddArrowMotions();

        if (Keybindings.NumpadMotionsEnabled)
            AddNumpadMotions();

        if (Keybindings.FPSMotionsEnabled)
            AddFPSMotions();
    }

    protected virtual void HandleEscape() { }

    protected void AddEscapeHandling()
    {
        SetAction(Keys.Escape, HandleEscape);
    }

    public virtual void UpdateKeybindings(ControlBase control)
    {
        if (control is CheckBox checkBox)
        {
            switch (checkBox.Text)
            {
                case MotionsSelectorScreen.ArrowButtonsText:
                    if (checkBox.IsSelected)
                        AddArrowMotions();
                    else
                        RemoveArrowMotions();
                    break;

                case MotionsSelectorScreen.NumpadButtonsText:
                    if (checkBox.IsSelected)
                        AddNumpadMotions();
                    else
                        RemovedNumpadMotions();
                    break;

                case MotionsSelectorScreen.FPSButtonsText:
                    if (checkBox.IsSelected)
                        AddFPSMotions();
                    else
                        RemoveFPSMotions();
                    break;

                case MotionsSelectorScreen.ViButtonsText:
                    if (checkBox.IsSelected)
                        AddViMotions();
                    else
                        RemoveViMotions();
                    break;
            }
        }
    }

    void AddViMotions() =>
    SetMotions(ViMotions);

    void AddArrowMotions() =>
        SetMotions(ArrowMotions);

    void AddNumpadMotions() =>
        SetMotions(NumPadAllMotions);

    void AddFPSMotions() =>
        SetMotions(WasdMotions);

    void RemoveViMotions() =>
        RemoveMotions(ViMotions.Select(e => e.binding));

    void RemoveArrowMotions() =>
        RemoveMotions(ArrowMotions.Select(e => e.binding));

    void RemovedNumpadMotions() =>
        RemoveMotions(NumPadAllMotions.Select(e => e.binding));

    void RemoveFPSMotions() =>
        RemoveMotions(WasdMotions.Select(e => e.binding));
}