using LuckNGold.Tests;
using LuckNGold.Visuals.Screens;
using SadConsole.Configuration;

namespace LuckNGold;

static class Program
{
    public const int Width = 84;
    public const int Height = 60;

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
                .SetStartingScreen<RootScreen>();

        // Setup the engine and start the game
        Game.Create(builder);
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