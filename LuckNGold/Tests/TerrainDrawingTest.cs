using GoRogue.Random;
using LuckNGold.Config;
using SadRogue.Primitives.GridViews;
using ShaiRandom.Generators;

namespace LuckNGold.Tests;

class TerrainDrawingTest : ScreenSurface
{
    readonly Grid _grid = new();
    readonly Rectangle _floorDecals = new(6, 0, 4, 4);
    string _lastBrickDividerSide = "Top";

    public TerrainDrawingTest() : base(13, 13)
    {
        Font = Game.Instance.Fonts["PixelDungeon"];
        FontSize *= 3;

        // draw grid
        for (int i = 0; i < _grid.Count; i++)
        {
            Point position = Point.FromIndex(i, _grid.Width);
            DrawTerrain(position);
        }
    }

    void DrawTerrain(Point position)
    {
        (int x, int y) = position;

        if (IsFloor(position))
        {
            Surface.SetBackground(x, y, Colors.Floor);
            DecorateFloor(x, y);
        }

        // cell is wall
        else
        {
            byte wallFlags = GetWallFlags(position);

            // wall is completely surrounded by other walls
            if (wallFlags == byte.MaxValue)
            {
                Surface.SetBackground(x, y, Colors.Wall);
            }
            else
            {
                GlyphDefinition glyphDefinition = GetWallAppearance(wallFlags);

                // set surface appearance
                Surface.SetGlyph(x, y, glyphDefinition.Glyph);
                Surface.SetMirror(x, y, glyphDefinition.Mirror);

                if (glyphDefinition.Glyph == Font.GetGlyphDefinition("TopWall").Glyph)
                    DecorateTopWall(x, y);
                else if (glyphDefinition.Glyph == Font.GetGlyphDefinition("BottomWall").Glyph)
                    DecorateBottomWall(x, y);
            }
        }
    }

    // draws floor decals
    void DecorateFloor(int x, int y)
    {
        var glyphPos = GlobalRandom.DefaultRNG.RandomPosition(_floorDecals);
        int glyph = glyphPos.ToIndex(10);
        Surface.SetGlyph(x, y, glyph);
    }

    // adds brick dividers and occasional spider webs
    void DecorateTopWall(int x, int y)
    {
        string brickDividerSide = _lastBrickDividerSide == "Top" ? "Bottom" : "Top";
        string brickDividerName = $"BrickDivider{brickDividerSide}";
        GlyphDefinition brickDivider = GetRandGlyphDefinitionWithName(brickDividerName);
        AddDecoratorToCell(x, y, brickDivider);

        bool addSpiderWeb = Game.Instance.Random.NextDouble() < 0.35;
        if (addSpiderWeb)
        {
            GlyphDefinition spiderWeb = GetRandGlyphDefinitionWithName("WallSpiderWeb");
            AddDecoratorToCell(x, y, spiderWeb);
        }

        _lastBrickDividerSide = brickDividerSide;
    }

    // adds bottom wall decals
    void DecorateBottomWall(int x, int y)
    {
        GlyphDefinition decal = GetRandGlyphDefinitionWithName("BottomWallDecal");
        AddDecoratorToCell(x, y, decal);
    }

    void AddDecoratorToCell(int x, int y, GlyphDefinition glyphDefinition, bool randomizeMirror = true)
    {
        var mirror = randomizeMirror ? 
            Game.Instance.Random.NextDouble() < 0.5 ? Mirror.None : Mirror.Horizontal 
            : glyphDefinition.Mirror;
        CellDecorator decorator = new(Color.White, glyphDefinition.Glyph, mirror);
        ColoredGlyphBase cell = Surface[x, y];
        CellDecoratorHelpers.AddDecorator(decorator, cell);
    }

    // returns wall appearance based on the flags set
    GlyphDefinition GetWallAppearance(byte wallFlags)
    {
        // TODO: compact it when all options are implemented
        return wallFlags switch
        {
            31 => Font.GetGlyphDefinition("TopWall"),       // 255-32-64-128
            159 => Font.GetGlyphDefinition("TopWall"),      // 255-32-64
            63 => Font.GetGlyphDefinition("TopWall"),       // 255-64-128
            191 => Font.GetGlyphDefinition("TopWall"),      // 255-64

            248 => Font.GetGlyphDefinition("BottomWall"),   // 255-1-2-4
            252 => Font.GetGlyphDefinition("BottomWall"),   // 255-1-2
            249 => Font.GetGlyphDefinition("BottomWall"),   // 255-2-4
            253 => Font.GetGlyphDefinition("BottomWall"),   // 255-2

            107 => Font.GetGlyphDefinition("LeftWall"),     // 255-4-16-128
            235 => Font.GetGlyphDefinition("LeftWall"),     // 255-4-16
            111 => Font.GetGlyphDefinition("LeftWall"),     // 255-16-128
            239 => Font.GetGlyphDefinition("LeftWall"),     // 255-16

            214 => Font.GetGlyphDefinition("RightWall"),    // 255-1-8-32
            246 => Font.GetGlyphDefinition("RightWall"),    // 255-1-8
            215 => Font.GetGlyphDefinition("RightWall"),    // 255-8-32
            247 => Font.GetGlyphDefinition("RightWall"),    // 255-8

            127 => Font.GetGlyphDefinition("LeftWall"),     // 255-128  top left corner
            223 => Font.GetGlyphDefinition("RightWall"),    // 255-32   top right corner
            251 => Font.GetGlyphDefinition("LeftCorner"),   // 255-4    bottom left corner
            254 => Font.GetGlyphDefinition("RightCorner"),  // 255-1    bottom right corner

            11 => Font.GetGlyphDefinition("TopWall"),       // 1+2+8    top left inner corner
            15 => Font.GetGlyphDefinition("TopWall"),       // 11+4
            43 => Font.GetGlyphDefinition("TopWall"),       // 11+32
            47 => Font.GetGlyphDefinition("TopWall"),       // 11+4+32

            22 => Font.GetGlyphDefinition("TopWall"),       // 2+4+16   top right inner corner
            23 => Font.GetGlyphDefinition("TopWall"),       // 22+1
            150 => Font.GetGlyphDefinition("TopWall"),      // 22+128
            151 => Font.GetGlyphDefinition("TopWall"),      // 22+1+128

            104 => Font.GetGlyphDefinition("BottomLeftInnerCorner"), // 8+32+64
            105 => Font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+1
            236 => Font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+128
            237 => Font.GetGlyphDefinition("BottomLeftInnerCorner"), // 104+1+128

            208 => Font.GetGlyphDefinition("BottomRightInnerCorner"), // 16+64+128
            212 => Font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+4
            240 => Font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+32
            244 => Font.GetGlyphDefinition("BottomRightInnerCorner"), // 208+4+32

            _ => Font.GetGlyphDefinition("Unknown")
        };
    }

    // get a random glyph definition that starts with the given name
    GlyphDefinition GetRandGlyphDefinitionWithName(string name)
    {
        var definitions = Font.GlyphDefinitions
            .Where(g => g.Key.StartsWith(name))
            .Select(g => g.Value)
            .ToArray();

        if (definitions.Length == 0)
            throw new Exception($"No glyph definitions found with name starting with '{name}'");

        return definitions[Game.Instance.Random.Next(definitions.Length)];
    }

    // set wall flags based on surrounding walls
    byte GetWallFlags(Point position)
    {
        // 1, 2, 4
        // 8, x, 16
        // 32,64,128

        byte wallFlags = 0;
        if (!IsFloor(position + Direction.Up))
            wallFlags |= 2;
        if (!IsFloor(position + Direction.Right))
            wallFlags |= 16;
        if (!IsFloor(position + Direction.Down))
            wallFlags |= 64;
        if (!IsFloor(position + Direction.Left))
            wallFlags |= 8;
        if (!IsFloor(position + Direction.UpLeft))
            wallFlags |= 1;
        if (!IsFloor(position + Direction.UpRight))
            wallFlags |= 4;
        if (!IsFloor(position + Direction.DownRight))
            wallFlags |= 128;
        if (!IsFloor(position + Direction.DownLeft))
            wallFlags |= 32;
        return wallFlags;
    }

    // Check if the position is a floor
    bool IsFloor(Point position)
    {
        (int x, int y) = position;
        int index = Point.ToIndex(x, y, _grid.Width);
        return index >= 0 && index < _grid.Count && !_grid[index];
    }
}

class Grid : GridView1DIndexBase<bool>
{
    readonly int[] _room =
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    };
    public override int Height => 13;
    public override int Width => 13;
    public override bool this[int index] => _room[index] == 1;
}