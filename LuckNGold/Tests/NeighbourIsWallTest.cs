using GoRogue.Random;
using SadConsole.Input;
using ShaiRandom.Generators;

namespace LuckNGold.Tests;

internal class NeighbourIsWallTest : SimpleSurface
{
    readonly Rectangle _testArea = new(0, 0, 20, 10);
    readonly Player _player;
    readonly ColoredGlyph _emptyCell = new(Color.White, Color.Transparent, ' ');
    readonly ScreenSurface _overlay;

    readonly Dictionary<Direction, byte> _wallFlags = new() {
        { Direction.Up, 2 },
        { Direction.Right, 16 },
        { Direction.Down, 64 },
        { Direction.Left, 8 },
        { Direction.UpLeft, 1 },
        { Direction.UpRight, 4 },
        { Direction.DownRight, 128 },
        { Direction.DownLeft, 32 }
    };

    public NeighbourIsWallTest()
    {
        IsFocused = true;
        _overlay = new(Width, Height);
        _overlay.Surface.DefaultForeground = Color.Red;
        _overlay.Surface.Clear();
        Children.Add(_overlay);

        // prepare test area
        int x = (Width - _testArea.Width) / 2;
        int y = (Height - _testArea.Height) / 2;
        _testArea = _testArea.WithPosition(new Point(x, y));
        var shapeParams = ShapeParameters.CreateStyledBoxThin(Color.CornflowerBlue);
        Surface.DrawBox(_testArea.Expand(1, 1), shapeParams);

        // spawn some walls
        for (int i = 0; i < 5; i++)
        {
            var pos = GlobalRandom.DefaultRNG.RandomPosition(_testArea);
            (x, y) = pos;
            Surface.SetGlyph(x, y, '#');
        }

        // prepare player
        _player = new(_testArea.Position);
        DrawPlayer();
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (!keyboard.HasKeysPressed)
            return false;

        var direction = Direction.None;
        if (keyboard.IsKeyPressed(Keys.Up))
            direction = Direction.Up;
        else if (keyboard.IsKeyPressed(Keys.Down))
            direction = Direction.Down;
        else if (keyboard.IsKeyPressed(Keys.Left))
            direction = Direction.Left;
        else if (keyboard.IsKeyPressed(Keys.Right))
            direction = Direction.Right;

        if (direction !=  Direction.None)
        {
            var pos = _player.Position + direction;
            if (_testArea.Contains(pos) && Surface.GetGlyph(pos.X, pos.Y) != '#')
            {
                ErasePlayer();
                _player.Position = pos;
                DrawPlayer();

                DrawNeighbourWallHits(pos);
                var wallFlags = GetNeighbourWallsAsByte(pos);
                (int x, int y) = _testArea.Position - (0, 4);
                Surface.Print(x, y, $"Flags: {wallFlags}  ");
            }
        }

        return base.ProcessKeyboard(keyboard);
    }

    void ErasePlayer()
    {
        (int x, int y) = _player.Position;
        Surface.SetCellAppearance(x, y, _emptyCell);
    }

    void DrawPlayer()
    {
        (int x, int y) = _player.Position;
        Surface.SetCellAppearance(x, y, _player.Appearance);
    }

    protected override void OnMouseLeftClicked(MouseScreenObjectState state)
    {
        if (_testArea.Contains(state.CellPosition))
        {
            (int x, int y) = state.CellPosition;
            var glyph = Surface.GetGlyph(x, y) == '#' ? ' ' : '#';
            Surface.SetGlyph(x, y, glyph);
        }
        base.OnMouseLeftClicked(state);
    }

    void DrawNeighbourWallHits(Point position)
    {
        _overlay.Surface.Clear();
        var neighbours = AdjacencyRule.EightWay.Neighbors(position);
        foreach (var point in neighbours)
        {
            if (NeighbourIsWall(point))
                _overlay.Surface.SetGlyph(point.X, point.Y, 'X');
        }
    }

    byte GetNeighbourWallsAsByte(Point position)
    {
        byte wallFlags = 0;

        var neighbours = AdjacencyRule.EightWay.Neighbors(position);
        foreach (var point in neighbours)
        {
            // check point is wall
            if (NeighbourIsWall(point))
            {
                var direction = Direction.GetDirection(position, point);
                var flag = _wallFlags[direction];
                wallFlags |= flag;
            }
        }

        return wallFlags;
    }

    bool NeighbourIsWall(Point position) =>
        !_testArea.Contains(position) || Surface[position].Glyph == '#';

    class Player(Point position)
    {
        public Point Position = position;
        public ColoredGlyph Appearance = new(Color.Yellow, Color.Transparent, '@');
    }
}