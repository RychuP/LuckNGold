using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Controls;

internal class CharacterWindowTabControl : TabControl
{
    public CharacterWindowTabControl(TabItem[] tabItems, int width, int height) : 
        base(tabItems, width, height)
    {
        Name = "Tabs";
    }
}