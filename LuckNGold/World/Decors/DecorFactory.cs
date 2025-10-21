using GoRogue.Random;
using LuckNGold.Primitives;
using LuckNGold.Visuals;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Decors;

static class DecorFactory
{
    /// <summary>
    /// Skull and bone that can be placed on the floor.
    /// </summary>
    public static RogueLikeEntity Skull() =>
        GetEntityWithRandMirror("Skull");

    /// <summary>
    /// Bones that can be placed on the floor.
    /// </summary>
    public static RogueLikeEntity Bones() =>
        GetEntityWithRandMirror("Bones");

    /// <summary>
    /// Boxes that can be placed on the floor.
    /// </summary>
    /// <param name="size">Size of the boxes: either large or small.</param>
    public static RogueLikeEntity Boxes(Size size) =>
        GetEntityWithRandMirror($"Boxes{size}", false);

    /// <summary>
    /// Spider web that can be placed on the floor in the corners of a room.
    /// </summary>
    /// <param name="orientation">Horizontal appearance of the web.</param>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static RogueLikeEntity SpiderWeb(HorizontalOrientation orientation) =>
        GetEntity($"FloorSpideWeb{orientation}");

    public static RogueLikeEntity Shackle(string size) =>
        GetEntity($"Shackle{size}");

    /// <summary>
    /// Steps leading to upper or lower levels.
    /// </summary>
    public static RogueLikeEntity Steps(bool faceRight, bool leadDown)
    {
        string direction = leadDown ? "Down" : "Up";
        string face = faceRight ? "Right" : "Left";
        return GetEntity($"Steps{direction}{face}");
    }

    /// <summary>
    /// Animated torch that can be placed on the side wall of a room.
    /// </summary>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static AnimatedRogueLikeEntity SideTorch(HorizontalOrientation orientation) =>
        GetAnimatedEntity($"SideTorch{orientation}");

    /// <summary>
    /// Animated torch that can be placed on the top wall of a room.
    /// </summary>
    public static AnimatedRogueLikeEntity Torch() =>
        GetAnimatedEntity("Torch");

    /// <summary>
    /// Free standing, burning candle on a non walkable stand that can be placed on the floor.
    /// </summary>
    /// <param name="size">Size of the stand.</param>
    public static AnimatedRogueLikeEntity Candle(Size size) =>
        GetAnimatedEntity($"{size}Candle", false);

    /// <summary>
    /// Animated flag that can be placed on the top wall of a room.
    /// </summary>
    /// <param name="color">Color of the inner portion of the flag that corresponds 
    /// to the given <see cref="Gemstone"/>.</param>
    public static AnimatedRogueLikeEntity Flag(Gemstone color) =>
        GetAnimatedEntity($"{color}Flag");

    public static AnimatedRogueLikeEntity FountainTop(string color) =>
        GetAnimatedEntity($"{color}FountainTop");

    public static AnimatedRogueLikeEntity FountainBottom(string color) =>
        GetAnimatedEntity($"{color}FountainBottom", false);

    static AnimatedRogueLikeEntity GetAnimatedEntity(string animationName, 
        bool isWalkable = true, string name = "") => 
        new(animationName, false, GameMap.Layer.Decor, isWalkable) 
            { Name = name != "" ? name : animationName};

    static RogueLikeEntity GetEntity(string name, bool isWalkable = true)
    {
        var glyphDef = Program.Font.GetGlyphDefinition(name);
        var appearance = glyphDef.CreateColoredGlyph();
        return new RogueLikeEntity(appearance, layer: (int)GameMap.Layer.Decor)
        {
            Name = name,
            IsWalkable = isWalkable
        };
    }
    static RogueLikeEntity GetEntityWithRandMirror(string name, bool isWalkable = true)
    {
        var mirror = GlobalRandom.DefaultRNG.NextBool() ? 0 : 2;
        var entity = GetEntity(name, isWalkable);
        entity.AppearanceSingle!.Appearance.Mirror = (Mirror)mirror;
        return entity;
    }
} 