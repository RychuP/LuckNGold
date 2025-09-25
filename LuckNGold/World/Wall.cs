using GoRogue.Random;
using LuckNGold.Visuals;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.World;

internal class Wall : MemoryAwareRogueLikeCell
{
    static string _lastBrickDividerSide = "Top";

    // 1, 2, 4
    // 8, x, 16
    // 32,64,128
    // flags used to compress all wall neighbours into one byte
    static readonly Dictionary<Direction, byte> _wallFlags = new() {
        { Direction.Up, 2 },
        { Direction.Right, 16 },
        { Direction.Down, 64 },
        { Direction.Left, 8 },
        { Direction.UpLeft, 1 },
        { Direction.UpRight, 4 },
        { Direction.DownRight, 128 },
        { Direction.DownLeft, 32 }
    };

    public Wall(Point position, IGridView<bool> grid) :
        base(position, Color.White, Colors.Wall, 0, (int)GameMap.Layer.Terrain, false, false)
    {
        var wallFlags = GetNeighbourAsFlags(position, grid);

        // any wall that is not completely surrounded by other walls
        if (wallFlags != byte.MaxValue)
        {
            GlyphDefinition glyphDefinition = GetWallAppearance(wallFlags);

            // set surface appearance
            Appearance.Glyph = glyphDefinition.Glyph;
            Appearance.Mirror = glyphDefinition.Mirror;

            var font = Program.Font;
            if (glyphDefinition.Glyph == font.GetGlyphDefinition("TopWall").Glyph)
                DecorateTopWall();
            else if (glyphDefinition.Glyph == font.GetGlyphDefinition("BottomWall").Glyph)
                DecorateBottomWall();
        }
    }

    // adds brick dividers and occasional spider webs
    // top wall is the wall at the top of the room with its inner, bright side visible
    void DecorateTopWall()
    {
        string brickDividerSide = _lastBrickDividerSide == "Top" ? "Bottom" : "Top";
        string brickDividerName = $"BrickDivider{brickDividerSide}";
        GlyphDefinition brickDivider = GetRandGlyphDefinitionWithName(brickDividerName);
        AddDecorator(brickDivider);

        bool addSpiderWeb = Game.Instance.Random.NextDouble() < 0.35;
        if (addSpiderWeb)
        {
            GlyphDefinition spiderWeb = GetRandGlyphDefinitionWithName("WallSpiderWeb");
            AddDecorator(spiderWeb);
        }

        _lastBrickDividerSide = brickDividerSide;
    }

    // adds bottom wall decals
    void DecorateBottomWall()
    {
        GlyphDefinition decal = GetRandGlyphDefinitionWithName("BottomWallDecal");
        AddDecorator(decal);
    }

    void AddDecorator(GlyphDefinition glyphDefinition, bool randomizeMirror = true)
    {
        var mirror = randomizeMirror ?
            (GlobalRandom.DefaultRNG.NextBool() ? Mirror.None : Mirror.Horizontal)
            : glyphDefinition.Mirror;
        CellDecorator decorator = new(Color.White, glyphDefinition.Glyph, mirror);

        if (Appearance.Decorators is null)
            Appearance.Decorators = [];
        Appearance.Decorators.Add(decorator);
    }

    // returns wall appearance based on the flags set
    static GlyphDefinition GetWallAppearance(byte wallFlags)
    {
        var font = Program.Font;

        // 1, 2, 4
        // 8, x, 16
        // 32,64,128
        // TODO: compact it when all options are implemented
        return wallFlags switch
        {
            31 => font.GetGlyphDefinition("TopWall"),       // 255-32-64-128
            159 => font.GetGlyphDefinition("TopWall"),      // 255-32-64
            63 => font.GetGlyphDefinition("TopWall"),       // 255-64-128
            191 => font.GetGlyphDefinition("TopWall"),      // 255-64

            248 => font.GetGlyphDefinition("BottomWall"),   // 255-1-2-4
            252 => font.GetGlyphDefinition("BottomWall"),   // 255-1-2
            249 => font.GetGlyphDefinition("BottomWall"),   // 255-2-4
            253 => font.GetGlyphDefinition("BottomWall"),   // 255-2

            107 => font.GetGlyphDefinition("LeftWall"),     // 255-4-16-128
            235 => font.GetGlyphDefinition("LeftWall"),     // 255-4-16
            111 => font.GetGlyphDefinition("LeftWall"),     // 255-16-128
            239 => font.GetGlyphDefinition("LeftWall"),     // 255-16

            214 => font.GetGlyphDefinition("RightWall"),    // 255-1-8-32
            246 => font.GetGlyphDefinition("RightWall"),    // 255-1-8
            215 => font.GetGlyphDefinition("RightWall"),    // 255-8-32
            247 => font.GetGlyphDefinition("RightWall"),    // 255-8

            127 => font.GetGlyphDefinition("LeftWall"),     // 255-128  top left corner
            223 => font.GetGlyphDefinition("RightWall"),    // 255-32   top right corner
            251 => font.GetGlyphDefinition("LeftCorner"),   // 255-4    bottom left corner
            254 => font.GetGlyphDefinition("RightCorner"),  // 255-1    bottom right corner

            11 => font.GetGlyphDefinition("TopWall"),       // 1+2+8    top left inner corner
            15 => font.GetGlyphDefinition("TopWall"),       // 11+4
            43 => font.GetGlyphDefinition("TopWall"),       // 11+32
            47 => font.GetGlyphDefinition("TopWall"),       // 11+4+32

            22 => font.GetGlyphDefinition("TopWall"),       // 2+4+16   top right inner corner
            23 => font.GetGlyphDefinition("TopWall"),       // 22+1
            150 => font.GetGlyphDefinition("TopWall"),      // 22+128
            151 => font.GetGlyphDefinition("TopWall"),      // 22+1+128

            104 => font.GetGlyphDefinition("BottomLeftInnerCorner"), // 8+32+64
            105 => font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+1
            232 => font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+128
            237 => font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+1+128

            208 => font.GetGlyphDefinition("BottomRightInnerCorner"), // 16+64+128
            212 => font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+4
            240 => font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+32
            244 => font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+4+32

            _ => font.GetGlyphDefinition("Unknown")
        };
    }

    // get a random glyph definition that starts with the given name
    static GlyphDefinition GetRandGlyphDefinitionWithName(string name)
    {
        var definitions = Program.Font.GlyphDefinitions
            .Where(g => g.Key.StartsWith(name))
            .Select(g => g.Value)
            .ToArray();

        if (definitions.Length == 0)
            throw new Exception($"No glyph definitions found with name starting with '{name}'");

        return definitions[Game.Instance.Random.Next(definitions.Length)];
    }

    /// <summary>
    /// Single byte representation of the neigbouring walls.
    /// </summary>
    /// <param name="wallPosition">Position of the wall to check neighbours of.</param>
    /// <returns>Byte representation of the wall with neighbours set as individual bits.</returns>
    public static byte GetNeighbourAsFlags(Point wallPosition, IGridView<bool> grid)
    {
        byte wallFlags = 0;

        var neighbourPositions = AdjacencyRule.EightWay.Neighbors(wallPosition);
        foreach (var position in neighbourPositions)
        {
            // check point is wall
            if (!grid.Contains(position) || !grid[position])
            {
                var direction = Direction.GetDirection(wallPosition, position);
                var flag = _wallFlags[direction];
                wallFlags |= flag;
            }
        }

        return wallFlags;
    }
}