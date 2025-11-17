namespace LuckNGold.Config;

internal class GameSettings
{
    public const string Title = "Luck N' Gold";
    public const bool DebugEnabled = true;
    public const int Width = 84;
    public const int Height = 60;
    public const int CharacterWindowWidth = 48;
    public const int CharacterWindowHeight = 29;

    public static readonly Point FontSize = (16, 16);
    public static Rectangle Bounds => new(0, 0, Width, Height);
    public static  Distance Distance => Distance.Chebyshev;
    public static AdjacencyRule Adjacency => AdjacencyRule.EightWay;
}