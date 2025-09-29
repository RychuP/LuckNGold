using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation;
using LuckNGold.Visuals;
using LuckNGold.World.Map.Components;
using SadConsole.Input;
using SadRogue.Integration.Keybindings;
using SadRogue.Integration.Maps;

namespace LuckNGold.World.Map;

/// <summary>
/// Map handles everything that happens inside the game world.
/// </summary>
class GameMap : RogueLikeMap
{
    // Map width/height
    public const int DefaultWidth = 100;
    public const int DefaultHeight = 60;

    // Effectively max view zoom level
    const int MaxFontSizeMultiplier = 4;

    // Effectively current view zoom level
    int _fontSizeMultiplier = 3;

    // List of all rooms from generator for testing/debugging
    public IReadOnlyList<Room> Rooms { get; init; }

    // List of all paths from generator for testing/debugging
    // (path connects many rooms in a logical, linear way)
    public IReadOnlyList<RoomPath> Paths { get; init; }

    // list of all corridors from generator for testing/debugging
    // (corridor connects two rooms)
    public IReadOnlyList<Corridor> Corridors { get; init; }

    /// <summary>
    /// Layers available for entities in the game
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

    // CUSTOMIZATION: Change the distance from Distance.Chebyshev to whatever is desired for your game. By default,
    // this will affect the FOV shape as well as the distance calculation used for AStar pathfinding on the Map.
    public GameMap(GenerationContext context) : base(context.Width, context.Height, 
        null, Enum.GetValues<Layer>().Length - 1, Distance.Chebyshev)
    {
        // TODO Delete this in the future when generation is sorted.
        // Generated data saved mainly for testing and debugging purposes.
        Paths = context.GetFirst<ItemList<RoomPath>>().Items;
        Corridors = context.GetFirst<ItemList<Corridor>>().Items;
        var temp = new List<Room>();
        foreach (var path in Paths)
            temp.AddRange(path.Rooms);
        Rooms = temp;

        // Create renderer
        Point viewSize = new(Program.Width / _fontSizeMultiplier,
            Program.Height / _fontSizeMultiplier);
        DefaultRenderer = CreateRenderer(viewSize);
        DefaultRenderer.Font = Program.Font;
        DefaultRenderer.FontSize *= _fontSizeMultiplier;

        // Change default bg color to match wall color
        DefaultRenderer.Surface.DefaultBackground = Colors.Wall;
        DefaultRenderer.Surface.Clear();

        // fov handler
        AllComponents.Add(new MapFOVHandler());
    }

    public void ResizeView(int fontSizeMultiplier)
    {
        if (fontSizeMultiplier < 0 || fontSizeMultiplier > 4) return;
        var width = Program.Width / fontSizeMultiplier;
        var height = Program.Height / fontSizeMultiplier;
        DefaultRenderer!.Surface.View = new Rectangle(0, 0, width, height);
        var size = DefaultRenderer.Font.GetFontSize(IFont.Sizes.One) * fontSizeMultiplier;
        DefaultRenderer.FontSize = size;
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
}