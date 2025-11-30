using LuckNGold.Config;
using SadConsole.EasingFunctions;
using SadConsole.Instructions;
using SadConsole.Renderers;

namespace LuckNGold.Visuals.Consoles;

internal class DamageNotification : ScreenSurface
{
    public DamageNotification(int length) : base(length, 1)
    {
        Surface.DefaultBackground = Theme.Floor;
        Surface.Clear();
        IsVisible = false;
    }

    public void Show(string text, Color color)
    {
        Surface.Clear();
        Surface.Print(0, 0, text, color);

        var animatedOpacity = new AnimatedValue(TimeSpan.FromSeconds(1d), 255, 0, new Linear())
        {
            RemoveOnFinished = true
        };

        animatedOpacity.ValueChanged += (o, d) =>
        {
            ((ScreenSurfaceRenderer)Renderer!).Opacity = (byte)d;
        };

        animatedOpacity.Finished += (o, e) =>
        {
            IsVisible = false;
        };

        SadComponents.Add(animatedOpacity);
        IsVisible = true;
    }
}