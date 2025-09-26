using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation;
using LuckNGold.Visuals;
using SadConsole.Input;
using SadRogue.Integration.Keybindings;
using SadRogue.Integration.Maps;

namespace LuckNGold.World;

/// <summary>
/// RogueLikeMap class that simplifies constructor and wraps map layers into a convenient, type-safe, 
/// customizable enumeration. Add/remove values from the enum as you like; 
/// the map will update accordingly to reflect number and order.
/// </summary>
internal class GameMap : RogueLikeMap
{
    // Map width/height
    public const int DefaultWidth = 100;
    public const int DefaultHeight = 60;

    // effectively max view zoom level
    const int MaxFontSizeMultiplier = 4;

    // effectively current view zoom level
    int _fontSizeMultiplier = 2;

    // list of all rooms from generator
    public IReadOnlyList<Room> Rooms { get; init; }

    // list of all paths from generator
    // path connects many rooms in a logical path
    public IReadOnlyList<RoomPath> Paths { get; init; }

    // list of all corridors from generator
    // corridor connects two rooms
    public IReadOnlyList<Corridor> Corridors { get; init; }

    // CUSTOMIZATION: Edit map layers here as desired; however ensure that Terrain stays as 0 to match GoRogue's
    // definition of the terrain layer.
    public enum Layer
    {
        // The basic structure of the level (walls, floors, corridors). 
        Terrain = 0,

        // Non-interactive visual elements like rubble, flags, torches and other env details. 
        Decor,

        // Visual elements that can be interacted with like doors, chests, levers, etc
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
        IsFocused = true;

        // save generated data for ease of use
        Paths = context.GetFirst<ItemList<RoomPath>>().Items;
        Corridors = context.GetFirst<ItemList<Corridor>>().Items;
        var temp = new List<Room>();
        foreach (var path in Paths)
            temp.AddRange(path.Rooms);
        Rooms = temp;

        // create renderer
        Point viewSize = new(Program.Width / _fontSizeMultiplier,
            Program.Height / _fontSizeMultiplier);
        DefaultRenderer = CreateRenderer(viewSize);
        DefaultRenderer.Font = Program.Font;
        DefaultRenderer.FontSize *= _fontSizeMultiplier;

        // change default bg color to match wall color
        DefaultRenderer.Surface.DefaultBackground = Colors.Wall;
        DefaultRenderer.Surface.Clear();

        // keyboard handler
        var actionHandler = new KeybindingsComponent<GameMap>();
        actionHandler.SetAction(Keys.C, ZoomViewIn);
        actionHandler.SetAction(Keys.Z, ZoomViewOut);
        AllComponents.Add(actionHandler);

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