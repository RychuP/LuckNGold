using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Map.Components;

namespace LuckNGold.Visuals.Components;

internal class CharacterWindowKeybindings : GameScreenKeybindingsComponent
{
    public CharacterWindowKeybindings(GameScreen gameScreen) : 
        base(gameScreen, gameScreen.Pointer)
    {
        
    }

    // Controls the pointer in the character window.
    protected override void MotionHandler(Direction direction)
    {
        base.MotionHandler(direction);
    }

    protected override void HandleEscape()
    {
        //GameScreen.HideCharacterWindow();
    }
}