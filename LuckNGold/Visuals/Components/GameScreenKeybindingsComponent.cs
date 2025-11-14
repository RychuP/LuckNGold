using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using LuckNGold.Visuals.Windows;
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

    /// <summary>
    /// Initializes an instance of <see cref="GameScreenKeybindingsComponent"/> class.
    /// </summary>
    /// <param name="gameScreen">Gamescreen parent.</param>
    /// <param name="motionTarget">Target for the motion controls.</param>
    public GameScreenKeybindingsComponent(GameScreen gameScreen, RogueLikeEntity motionTarget)
    {
        GameScreen = gameScreen;
        MotionTarget = motionTarget;
        AddCharacterWindowControls();
    }

    /// <summary>
    /// Keyboard shortcut that toggles <see cref="CharacterWindow"/> visibility.
    /// </summary>
    void AddCharacterWindowControls()
    {
        SetAction(Keybindings.CharacterWindow, GameScreen.ShowCharacterWindow);
    }

    /// <summary>
    /// Map zoom controls.
    /// </summary>
    protected void AddMapZoomControls()
    {
        SetAction(Keybindings.ZoomIn, GameScreen.Map.ZoomViewIn);
        SetAction(Keybindings.ZoomOut, GameScreen.Map.ZoomViewOut);
    }

    /// <summary>
    /// Keyboard shortcuts relating to debug overlay screens.
    /// </summary>
    protected void AddDebugControls()
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

    /// <summary>
    /// Escape button behaviour.
    /// </summary>
    protected override void HandleEscape()
    {
        Program.RootScreen.Show<PauseScreen>();
    }
}