using GoRogue.Random;
using LuckNGold.Visuals;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.World.Terrain;

internal class Wall : RogueLikeCell
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
            GlobalRandom.DefaultRNG.NextBool() ? Mirror.None : Mirror.Horizontal
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
        return wallFlags switch
        {
            11 or 15 or 22 or 23 or 31 or 43 or 47 or 63 or 150 or 151 or 159 or 191 =>
                font.GetGlyphDefinition("TopWall"),

            248 or 249 or 252 or 253 =>
                font.GetGlyphDefinition("BottomWall"),

            107 or 111 or 127 or 235 or 239 =>
                font.GetGlyphDefinition("LeftWall"),

            214 or 215 or 223 or 246 or 247 =>
                font.GetGlyphDefinition("RightWall"),

            104 or 105 or 232 or 237 =>
                font.GetGlyphDefinition("BottomLeftInnerCorner"),

            208 or 212 or 240 or 244 =>
                font.GetGlyphDefinition("BottomRightInnerCorner"),

            251 =>
                font.GetGlyphDefinition("LeftCorner"),

            254 =>
                font.GetGlyphDefinition("RightCorner"),

            _ => 
                font.GetGlyphDefinition("Unknown")
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