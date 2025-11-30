namespace LuckNGold.Config;

internal class GameSettings
{
    /// <summary>
    /// Title of the game.
    /// </summary>
    public const string Title = "Luck N' Gold";

    /// <summary>
    /// Whether debug is enabled or not.
    /// </summary>
    public const bool DebugEnabled = true;

    /// <summary>
    /// Width of the game window in cells.
    /// </summary>
    public const int Width = 84;

    /// <summary>
    /// Height of the game window in cells.
    /// </summary>
    public const int Height = 60;

    public const int CharacterWindowWidth = 48;
    public const int CharacterWindowHeight = 29;

    /// <summary>
    /// Standard unit of time allocated to entities per turn.
    /// </summary>
    public const int TurnTime = 100;

    public static readonly Point FontSize = (16, 16);

    /// <summary>
    /// Bounds of the game window.
    /// </summary>
    public static Rectangle Bounds => new(0, 0, Width, Height);

    public static  Distance Distance => Distance.Chebyshev;
    public static AdjacencyRule Adjacency => AdjacencyRule.EightWay;
}