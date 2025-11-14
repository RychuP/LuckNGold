using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Windows.Panels;

internal class CharacterWindowSlot : ControlBase
{
    public CharacterWindowSlot(int slotSize) : base(slotSize, slotSize)
    {
        CanResize = false;
    }

    void DrawBorder()
    {
        var borderColor = ThemeState.GetStateAppearance(State).Foreground;
        var shapeParameters = ShapeParameters.CreateStyledBoxThin(borderColor);
        var itemBorder = new Rectangle(0, 0, Width, Height);
        Surface.DrawBox(itemBorder, shapeParameters);
    }

    protected override void OnStateChanged(ControlStates oldState, ControlStates newState)
    {
        base.OnStateChanged(oldState, newState);
    }

    public override void UpdateAndRedraw(TimeSpan time)
    {
        if (!IsDirty) return;
        Colors colors = FindThemeColors();
        RefreshThemeStateColors(colors);
        DrawBorder();
    }
}