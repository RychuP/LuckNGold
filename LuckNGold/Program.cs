using LuckNGold.Tests;
using LuckNGold.Visuals.Screens;
using SadConsole.Configuration;

namespace LuckNGold;

static class Program
{
    public const int Width = 84;
    public const int Height = 60;

    public static RootScreen? RootScreen { get; private set; }
    public static IFont Font { get; } = Game.Instance.Fonts["PixelDungeon"];
    public static Distance Distance { get; } = Distance.Chebyshev;
    public static AdjacencyRule Adjacency { get; } = AdjacencyRule.EightWay;

    public static Color RandomColor =>
        Color.White.GetRandomColor(Game.Instance.Random);

    static void Main()
    {
        Settings.WindowTitle = "Luck N' Gold";
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;

        // Configure how SadConsole starts up
        Builder builder = new Builder()
                .SetScreenSize(Width, Height)
                .ConfigureFonts(FontLoader)
                .OnStart(Init);

        // Setup the engine and start the game
        Game.Create(builder);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    static void Init(object? s, GameHost host)
    {
        RootScreen = new RootScreen();
        host.Screen = RootScreen;
        //host.Screen = new Test();
    }

    static void FontLoader(FontConfig fontConfig, GameHost host)
    {
        string C64Path = Path.Combine("Resources", "Fonts", "C64.font");
        string PixelDungeonPath = Path.Combine("Resources", "Fonts", "PixelDungeon.font");
        fontConfig.UseCustomFont(C64Path);
        fontConfig.AddExtraFonts([PixelDungeonPath]);
    }
}