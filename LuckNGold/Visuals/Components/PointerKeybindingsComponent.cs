using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using SadConsole.Input;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Keybindings for pointer in the look around the map mode.
/// </summary>
internal class PointerKeybindingsComponent : GameScreenKeybindingsComponent
{
    /// <summary>
    /// Initializes an instace of <see cref="PointerKeybindingsComponent"/> class.
    /// </summary>
    /// <param name="gameScreen"></param>
    public PointerKeybindingsComponent(GameScreen gameScreen) 
        : base(gameScreen, gameScreen.Pointer)
    {
        AddMapZoomControls();
        AddDebugControls();
        AddPointerControls();
    }

    /// <summary>
    /// Keyboard shortcuts relating to the pointer.
    /// </summary>
    void AddPointerControls()
    {
        SetAction(Keybindings.Look, GameScreen.HidePointer);
        SetAction(Keybindings.Interact, GameScreen.ToggleEntityInfo);
    }

    // Motion handler for the pointer movement.
    protected override void MotionHandler(Direction direction)
    {
        var position = MotionTarget.Position + direction;
        if (!MotionTarget.CurrentMap!.Contains(position))
            return;
        MotionTarget.Position = position;
    }

    protected override void HandleEscape()
    {
        GameScreen.HideEntityInfoOrPointer();
    }
}