using LuckNGold.Config;
using SadConsole.EasingFunctions;
using SadConsole.Instructions;
using SadConsole.Renderers;

namespace LuckNGold.Visuals.Consoles;

internal class DamageNotification : ScreenSurface
{
    public DamageNotification(string text, Color color) : base(text.Length, 1)
    {
        Surface.DefaultBackground = Theme.Floor;
        Surface.Clear();
        Surface.Print(0, 0, text, Color.White);
        AnimateOpacity();
    }

    void AnimateOpacity()
    {
        var animatedOpacity = new AnimatedValue(TimeSpan.FromSeconds(1.2d), 255, 0, new Linear())
        {
            RemoveOnFinished = true
        };

        animatedOpacity.ValueChanged += (o, d) =>
        {
            ((ScreenSurfaceRenderer)Renderer!).Opacity = (byte)d;
        };

        animatedOpacity.Finished += (o, e) =>
        {
            Parent!.Children.Remove(this);
        };

        SadComponents.Add(animatedOpacity);
    }
}