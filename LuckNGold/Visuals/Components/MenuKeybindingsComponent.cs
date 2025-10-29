
using LuckNGold.Visuals.Screens;

namespace LuckNGold.Visuals.Components;

internal class MenuKeybindingsComponent : KeybindingsComponentBase
{
    public MenuKeybindingsComponent()
    {

    }

    protected override void MotionHandler(Direction direction)
    {
        if (Parent is not MenuScreen menuScreen) return;

        if (direction == Direction.Up)
            menuScreen.Controls.TabPreviousControl();
        else if (direction == Direction.Down)
            menuScreen.Controls.TabNextControl();
    }
}