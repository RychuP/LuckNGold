using LuckNGold.Visuals.Screens;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Common GameScreen keyboard controls.
/// </summary>
// TODO some methods don't really belong here. Move them to more appropriate classes.
abstract class GameScreenKeybindingsComponent : KeybindingsComponentBase
{
    protected RogueLikeEntity MotionTarget { get; }
    protected GameScreen GameScreen { get; }

    protected readonly static Keys[] QuickAccessKeys = [Keys.D0, Keys.D1, Keys.D2, Keys.D3, 
        Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9];

    public GameScreenKeybindingsComponent(GameScreen gameScreen, RogueLikeEntity motionTarget)
    {
        GameScreen = gameScreen;
        MotionTarget = motionTarget;

        // Add common controls.
        AddPointerControls();
        AddMapZoomControls();
        AddDebugControls();
    }

    protected virtual void AddPointerControls() { }

    void AddMapZoomControls()
    {
        SetAction(Keys.C, GameScreen.Map.ZoomViewIn);
        SetAction(Keys.Z, GameScreen.Map.ZoomViewOut);
    }


    // Adds keyboard shortcuts to show debug overlay screens.
    void AddDebugControls()
    {
        if (!GameScreen.DebugEnabled) return;

        // Debug console on and off
        var inputKey = new InputKey(Keys.OemTilde, KeyModifiers.LeftShift);
        SetAction(inputKey, () => GameScreen.DebugConsole.IsVisible
            = !GameScreen.DebugConsole.IsVisible);

        // Map layout on and off
        inputKey = new InputKey(Keys.OemTilde, KeyModifiers.LeftCtrl);
        SetAction(inputKey, () =>
        {
            if (GameScreen.MapLayout is null) return;
            GameScreen.MapLayout.IsVisible = !GameScreen.MapLayout.IsVisible;
        });
    }

    protected override void HandleEscape()
    {
        Program.RootScreen.Show<PauseScreen>();
    }
}