using LuckNGold.Config;
using LuckNGold.Tests;
using LuckNGold.Visuals.Screens;
using SadConsole.Configuration;

namespace LuckNGold;

static class Program
{
    public static RootScreen RootScreen { get; } = new();
    public static IFont Font => Game.Instance.Fonts["Dungeon"];
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
        Settings.WindowTitle = GameSettings.Title;
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;

        // Configure how SadConsole starts up.
        Builder builder = new Builder()
            .SetScreenSize(GameSettings.Width, GameSettings.Height)
            .ConfigureFonts(FontLoader)
            .OnStart(RootScreen.Init);
            //.SetStartingScreen<Test>();

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
        string C64FontPath = Path.Combine("Resources", "Fonts", "C64.font");
        string DungeonFontPath = Path.Combine("Resources", "Fonts", "Dungeon.font");
        fontConfig.UseCustomFont(C64FontPath);
        fontConfig.AddExtraFonts([DungeonFontPath]);
        LoadChibiFonts(host);
    }

    /// <summary>
    /// Loads all chibi sprite sheets as fonts.
    /// </summary>
    static void LoadChibiFonts(GameHost host)
    {
        var chibiFilesPath = Path.Combine("Resources", "Fonts", "Chibi");
        var enumarator = Directory.EnumerateFiles(chibiFilesPath);

        foreach (var file in enumarator)
        {
            var fontTexture = GameHost.Instance.GetTexture(file);
            int rowCount = fontTexture.Height / 16;
            int columnCount = fontTexture.Width / 16;
            var name = Path.GetFileNameWithoutExtension(file);
            SadFont font = new(GameSettings.FontSize.X, GameSettings.FontSize.Y, 0, 
                rowCount, columnCount, 1, fontTexture, name);
            host.Fonts[name] = font;
        }
    }
}