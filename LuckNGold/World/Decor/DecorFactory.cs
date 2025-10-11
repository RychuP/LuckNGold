using GoRogue.Random;
using LuckNGold.Visuals;
using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace LuckNGold.World.Decor;

static class DecorFactory
{
    /// <summary>
    /// Animated flag that can be placed on the top wall of a room.
    /// </summary>
    /// <param name="color">Color of the inner portion of the flag that corresponds 
    /// to the given <see cref="Gemstone"/>.</param>
    public static RogueLikeEntity Flag(Gemstone color)
    {
        return new AnimatedRogueLikeEntity($"{color}Flag", false, GameMap.Layer.Decor)
        {
            Name = $"{color} Flag"
        };
    }

    /// <summary>
    /// Free standing, burning candle on a non walkable stand that can be placed on the floor.
    /// </summary>
    /// <param name="size">Size of the stand.</param>
    public static RogueLikeEntity Candle(Size size)
    {
        return new AnimatedRogueLikeEntity($"{size}Candle", false, GameMap.Layer.Decor)
        {
            Name = $"{size} Candle",
            IsWalkable = false
        };
    }

    /// <summary>
    /// Animated torch that can be placed on the top wall of a room.
    /// </summary>
    public static RogueLikeEntity Torch()
    {
        return new AnimatedRogueLikeEntity("Torch", false, GameMap.Layer.Decor)
        {
            Name = "Torch"
        };
    }

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
    /// <param name="side">Horizontal appearance of the web.</param>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static RogueLikeEntity SpiderWeb(Direction side)
    {
        if (!side.IsHorizontal())
            throw new ArgumentException("Side needs to be horizontal.");
        return GetEntity($"FloorSpideWeb{side}");
    }

    /// <summary>
    /// Animated torch that can be placed on the side wall of a room.
    /// </summary>
    /// <param name="side">Either left or right side of the room.</param>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static RogueLikeEntity SideTorch(Direction side)
    {
        if (!side.IsHorizontal())
            throw new ArgumentException("Side needs to be horizontal.");

        return new AnimatedRogueLikeEntity($"SideTorch{side}", false, GameMap.Layer.Decor)
        {
            Name = "Side Torch"
        };
    }

    /// <summary>
    /// Steps leading to upper or lower levels.
    /// </summary>
    /// <param name="side">Horizontal appearance: either left or right.</param>
    /// <param name="direction">Vertical appearance: either up or down.</param>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static RogueLikeEntity Steps(Direction side, Direction direction)
    {
        if (!direction.IsVertical())
            throw new ArgumentException("Direction needs to be vertical.");

        if (!side.IsHorizontal())
            throw new ArgumentException("Side needs to be horizontal");

        return GetEntity($"Steps{direction}{side}");
    }

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
        var rand = GlobalRandom.DefaultRNG;
        var mirror = rand.NextBool() ? 0 : 2;
        var entity = GetEntity(name, isWalkable);
        entity.AppearanceSingle!.Appearance.Mirror = (Mirror)mirror;
        return entity;
    }
} 