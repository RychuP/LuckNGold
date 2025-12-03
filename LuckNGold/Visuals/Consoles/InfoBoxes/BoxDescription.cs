using LuckNGold.Config;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

internal class BoxDescription : ScreenSurface
{
    public BoxDescription(string text) : base(text.Length + 2, 3)
    {
        Hide();
        if (string.IsNullOrEmpty(text)) return;
        Surface.Print(1, 1, text, Theme.Colors.Gray);
        var shapeParams = ShapeParameters.CreateStyledBoxThin(Theme.Colors.Lines);
        Surface.DrawBox(Surface.Area, shapeParams);
    }

    public void Show() => IsVisible = true;
    public void Hide() => IsVisible = false;
}