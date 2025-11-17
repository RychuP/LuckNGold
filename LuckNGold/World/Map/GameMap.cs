using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Config;
using LuckNGold.Generation.Map;
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

    /// <summary>
    /// Default map width.
    /// </summary>
    public const int DefaultWidth = 100;

    /// <summary>
    /// Default map height.
    /// </summary>
    public const int DefaultHeight = 60;

    /// <summary>
    /// Font size multiplier that is first applied when the game starts.
    /// </summary>
    public const int DefaultFontSizeMultiplier = 3;

    /// <summary>
    /// Max font size multiplier that also defines max view zoom level.
    /// </summary>
    const int MaxFontSizeMultiplier = 4;

    /// <summary>
    /// Current font size multiplier that also defines current view zoom level.
    /// </summary>
    public int FontSizeMultiplier { get; private set; } = DefaultFontSizeMultiplier;

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
    /// All <see cref="RogueLikeEntity"/>s on <see cref="Layer.Monsters"/>.
    /// </summary>
    public IEnumerable<RogueLikeEntity> Monsters => Entities
        .Where(e => e.Item.Layer == (int)Layer.Monsters)
        .Select(e => e.Item)
        .Cast<RogueLikeEntity>();

    /// <summary>
    /// Initializes an instance of <see cref="GameMap"/> with parameters provided.
    /// </summary>
    /// <param name="context">Generation context.</param>
    public GameMap(GenerationContext context) : base(context.Width, context.Height, 
        null, Enum.GetValues<Layer>().Length - 1, GameSettings.Distance)
    {
        // Save generated data for testing and debugging purposes.
        Paths = GameSettings.DebugEnabled ? context.GetFirst<ItemList<RoomPath>>().Items : [];
        Sections = GameSettings.DebugEnabled ? context.GetFirst<ItemList<Section>>().Items : [];

        // Create renderer.
        Point viewSize = new(GameSettings.Width / FontSizeMultiplier,
            GameSettings.Height / FontSizeMultiplier);
        DefaultRenderer = CreateRenderer(viewSize);
        DefaultRenderer.Font = Program.Font;
        DefaultRenderer.FontSize *= FontSizeMultiplier;

        // Change default bg color to match wall color.
        DefaultRenderer.Surface.DefaultBackground = Theme.Wall;
        DefaultRenderer.Surface.Clear();

        // FOV handler.
        AllComponents.Add(new MapFOVHandler());
    }

    /// <summary>
    /// Resizes view of the map in response to zooming in and out.
    /// </summary>
    void ResizeView(int fontSizeMultiplier)
    {
        var width = GameSettings.Width / fontSizeMultiplier;
        var height = GameSettings.Height / fontSizeMultiplier;
        DefaultRenderer!.Surface.View = new Rectangle(0, 0, width, height);
        var size = DefaultRenderer.Font.GetFontSize(IFont.Sizes.One) * fontSizeMultiplier;
        DefaultRenderer.FontSize = size;
        OnViewZoomChanged();
    }

    public void ZoomViewIn()
    {
        if (FontSizeMultiplier >= MaxFontSizeMultiplier) return;
        ResizeView(++FontSizeMultiplier);
    }

    public void ZoomViewOut()
    {
        if (FontSizeMultiplier <= 1) return;
        ResizeView(--FontSizeMultiplier);
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