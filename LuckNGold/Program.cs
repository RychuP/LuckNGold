using LuckNGold.Tests;
using LuckNGold.Visuals.Screens;
using SadConsole.Configuration;

namespace LuckNGold;

static class Program
{
    public const int Width = 84;
    public const int Height = 60;

    public static RootScreen RootScreen { get; } = new();
    public static Rectangle Bounds => new(0, 0, Width, Height);
    public static IFont Font => Game.Instance.Fonts["PixelDungeon"];
    public static Distance Distance => Distance.Chebyshev;
    public static AdjacencyRule Adjacency => AdjacencyRule.EightWay;
    public static Color RandomColor => Color.White.GetRandomColor(Game.Instance.Random);
    public static Color RandomBrightColor
    {
        get 
        {
            Color color;
            do color = RandomColor;
            while (color.GetHSVBrightness() < 0.5f);
            return color;
        }
    }

    static void Main()
    {
        Settings.WindowTitle = "Luck N' Gold";
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;

        // Configure how SadConsole starts up
        Builder builder = new Builder()
                .SetScreenSize(Width, Height)
                .ConfigureFonts(FontLoader)
                .OnStart(RootScreen.Init);

        // Setup the engine.
        Game.Create(builder);

        // Reduce key pressed delay.
        Game.Instance.Keyboard.InitialRepeatDelay = 0.2f;
        Game.Instance.Keyboard.RepeatDelay = 0.15f;

        // Start the game.
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    static void FontLoader(FontConfig fontConfig, GameHost host)
    {
        string C64Path = Path.Combine("Resources", "Fonts", "C64.font");
        string PixelDungeonPath = Path.Combine("Resources", "Fonts", "PixelDungeon.font");
        fontConfig.UseCustomFont(C64Path);
        fontConfig.AddExtraFonts([PixelDungeonPath]);
    }
}