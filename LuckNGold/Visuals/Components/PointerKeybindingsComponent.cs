using LuckNGold.Visuals.Screens;
using SadConsole.Input;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.Visuals.Components;

internal class PointerKeybindingsComponent : GameScreenKeybindingsComponent
{
    public PointerKeybindingsComponent(GameScreen gameScreen) 
        : base(gameScreen, gameScreen.Pointer)
    {
        
    }

    protected override void AddPointerControls()
    {
        SetAction(Keys.X, GameScreen.HidePointer);
        SetAction(Keys.Escape, GameScreen.ClosePopUpOrHidePointer);
        SetAction(Keys.F, GameScreen.ToggleEntityInfo);
    }

    // Motion handler for the pointer movement.
    protected override void MotionHandler(Direction direction)
    {
        var position = MotionTarget.Position + direction;
        if (!MotionTarget.CurrentMap!.Contains(position))
            return;
        MotionTarget.Position = position;
    }
}