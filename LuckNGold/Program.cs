using LuckNGold.Tests;
using SadConsole.Configuration;

namespace LuckNGold;

/// <summary>
/// The provided code is a simple template to demonstrate some integration library features 
/// and set up some boilerplate for you. Feel free to use or delete any of it that you want; 
/// it shows one way of doing things, not the only way!
///
/// The code contains a few comments beginning with "CUSTOMIZATION:", which show you some common points 
/// to modify in order to accomplish some common tasks. The tags by no means represent a comprehensive guide 
/// to cover everything you might want to modify; they're simply designed to provide a "quick-start" guide 
/// that can help you accomplish some common tasks.
/// </summary>
static class Program
{
    public const int Width = 60;
    public const int Height = 50;

    public static RootScreen? RootScreen;
    public static IFont MainFont = Game.Instance.Fonts["PixelDungeon"];

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
    }

    static void FontLoader(FontConfig fontConfig, GameHost host)
    {
        string C64Path = Path.Combine("Resources", "Fonts", "C64.font");
        string PixelDungeonPath = Path.Combine("Resources", "Fonts", "PixelDungeon.font");
        fontConfig.UseCustomFont(C64Path);
        fontConfig.AddExtraFonts([PixelDungeonPath]);
    }

    
}