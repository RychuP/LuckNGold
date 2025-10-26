using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation.Map;
using LuckNGold.Visuals;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Map.Components;
using SadRogue.Integration;
using SadRogue.Integration.Maps;

namespace LuckNGold.World.Map;

/// <summary>
/// Object that represents and holds all the data about the current level.
/// </summary>
partial class GameMap : RogueLikeMap
{
    public event EventHandler? ViewZoomChanged;

    // Map width/height
    public const int DefaultWidth = 100;
    public const int DefaultHeight = 60;

    // Effectively max view zoom level
    const int MaxFontSizeMultiplier = 4;

    // Effectively current view zoom level
    int _fontSizeMultiplier = 3;

    /// <summary>
    /// List of paths from generator (available only if debugging is enabled).
    /// </summary>
    public IReadOnlyList<RoomPath> Paths { get; }

    /// <summary>
    /// List of sections from generator (available only if debugging is enabled).
    /// </summary>
    public IReadOnlyList<Section> Sections { get; }

    /// <summary>
    /// Layers available for entities in the game.
    /// </summary>
    public enum Layer
    {
        // The basic structure of the level (walls, floors, corridors). 
        Terrain = 0,

        // Non-interactive entities like rubble, flags, torches and other env details. 
        Decor,

        // Entities that can be interacted with like doors, chests, levers, etc
        Furniture,

        // Loot, tools, and other collectibles that can be picked up. Also projectiles ??
        Items,

        // Non-player characters, enemies, and the player itself. 
        Monsters
    }

    /// <summary>
    /// Initializes an instance of <see cref="GameMap"/> with parameters provided.
    /// </summary>
    /// <param name="context">Generation context.</param>
    public GameMap(GenerationContext context) : base(context.Width, context.Height, 
        null, Enum.GetValues<Layer>().Length - 1, Program.Distance)
    {
        // Save generated data for testing and debugging purposes.
        Paths = GameScreen.DebugEnabled ? context.GetFirst<ItemList<RoomPath>>().Items : [];
        Sections = GameScreen.DebugEnabled ? context.GetFirst<ItemList<Section>>().Items : [];

        // Create renderer.
        Point viewSize = new(Program.Width / _fontSizeMultiplier,
            Program.Height / _fontSizeMultiplier);
        DefaultRenderer = CreateRenderer(viewSize);
        DefaultRenderer.Font = Program.Font;
        DefaultRenderer.FontSize *= _fontSizeMultiplier;

        // Change default bg color to match wall color.
        DefaultRenderer.Surface.DefaultBackground = Colors.Wall;
        DefaultRenderer.Surface.Clear();

        // FOV handler.
        AllComponents.Add(new MapFOVHandler());
    }

    /// <summary>
    /// Resizes view of the map in response to zooming in and out.
    /// </summary>
    /// <param name="fontSizeMultiplier"></param>
    public void ResizeView(int fontSizeMultiplier)
    {
        if (fontSizeMultiplier < 0 || fontSizeMultiplier > 4) return;
        var width = Program.Width / fontSizeMultiplier;
        var height = Program.Height / fontSizeMultiplier;
        DefaultRenderer!.Surface.View = new Rectangle(0, 0, width, height);
        var size = DefaultRenderer.Font.GetFontSize(IFont.Sizes.One) * fontSizeMultiplier;
        DefaultRenderer.FontSize = size;
        OnViewZoomChanged();
    }

    public void ZoomViewIn()
    {
        if (_fontSizeMultiplier >= MaxFontSizeMultiplier) return;
        ResizeView(++_fontSizeMultiplier);
    }

    public void ZoomViewOut()
    {
        if (_fontSizeMultiplier <= 1) return;
        ResizeView(--_fontSizeMultiplier);
    }

    /// <summary>
    /// Adds an entity and sets its visibility to false.
    /// </summary>
    public void AddEntity(RogueLikeEntity entity, Point position)
    {
        entity.Position = position;
        entity.IsVisible = false;
        AddEntity(entity);
    }

    void OnViewZoomChanged()
    {
        ViewZoomChanged?.Invoke(this, EventArgs.Empty);
    }
}