using LuckNGold.Generation;
using SadConsole;
using SadConsole.Input;

namespace LuckNGold.Tests;

internal class ProbeTest : SimpleSurface
{
    const int CorridorLength = 3;

    readonly ShapeParameters _roomBorderShapeParams;
    readonly ShapeParameters _dummyRoomBorderShapeParams;
    readonly ShapeParameters _probeBorderShapeParams;
    readonly List<Room> _roomList;

    ColoredGlyph _probeBorderAppearance;
    Direction _probeDirection = Direction.Right;
    Probe _probe;

    Room _dummy;

    Room _room;
    Room Room
    {
        get => _room;
        set
        {
            _room = value;
            OnRoomChanged(value);
        }
    }

    public ProbeTest()
    {
        IsFocused = true;

        var borderStyle = new ColoredGlyph(Color.CornflowerBlue, Color.Transparent, 177);
        var areaStyle = new ColoredGlyph(Color.LightGray, Color.Transparent, Font.SolidGlyphIndex);
        var dummyStyle = new ColoredGlyph(Color.LightCoral, Color.Transparent, Font.SolidGlyphIndex);
        _probeBorderAppearance = new(Color.LightGreen, Color.Transparent, 178);
        _roomBorderShapeParams = ShapeParameters.CreateFilled(borderStyle, areaStyle);
        _dummyRoomBorderShapeParams = ShapeParameters.CreateFilled(borderStyle, dummyStyle);
        _probeBorderShapeParams = ShapeParameters.CreateBorder(_probeBorderAppearance);

        _room = new Room(10, 10, 7, 5);
        _probe = new Probe(_room, _probeDirection, CorridorLength);

        // dummy rooms
        _dummy = new Room(30, 20, 7, 5);
        _roomList = [_dummy];

        DrawAll();
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (!keyboard.HasKeysPressed)
            return false;

        Direction direction = Direction.None;
        if (keyboard.IsKeyPressed(Keys.Up))
            direction = Direction.Up;
        else if (keyboard.IsKeyPressed(Keys.Down))
            direction = Direction.Down;
        else if (keyboard.IsKeyPressed(Keys.Left))
            direction = Direction.Left;
        else if (keyboard.IsKeyPressed (Keys.Right))
            direction = Direction.Right;

        if (direction != Direction.None)
        {
            // move the probe with shift
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                _probeDirection = direction;
                _probe = new(_room, direction, CorridorLength);
            }
            // move the dummy with ctrl
            else if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                var pos = _dummy.Position + (direction.DeltaX, direction.DeltaY);
                _dummy = new(pos, _dummy.Width, _dummy.Height);
                _roomList.Clear();
                _roomList.Add(_dummy);
            }
            // move the main room
            else
            {
                var pos = Room.Position + (direction.DeltaX, direction.DeltaY);
                Room = new(pos, Room.Width, Room.Height);
            }

            // do the probing business
            try 
            {
                _probeBorderAppearance.Foreground = Color.LightGreen;
                _probe.CheckArea(_roomList, Surface.Area); 

            }
            catch (ProbeException)
            {
                _probeBorderAppearance.Foreground = Color.Red;
            }

            DrawAll();
        }
        else if (keyboard.IsKeyPressed(Keys.Space))
        {
            DrawSideEdges();
            DrawOuterEdge();
        }

        return base.ProcessKeyboard(keyboard);
    }

    void DrawAll()
    {
        Surface.Clear();
        DrawRooms();
        DrawProbe();
    }

    void DrawRooms()
    {
        Surface.DrawBox(Room.Bounds, _roomBorderShapeParams);

        foreach (var room in _roomList)
        {
            Surface.DrawBox(room.Bounds, _dummyRoomBorderShapeParams);
        }
    }

    void DrawProbe()
    {
        Surface.DrawBox(_probe.Bounds, _probeBorderShapeParams);
    }

    void DrawSideEdges()
    {
        foreach (var point in _probe.SideEdges)
            Surface.Print(point.X, point.Y, "$", Color.AnsiCyan);
    }

    void DrawOuterEdge()
    {
        foreach (var point in _probe.OuterEdge)
            Surface.Print(point.X, point.Y, "O", Color.AnsiMagenta);
    }

    void OnRoomChanged(Room newRoom)
    {
        _probe = new Probe(newRoom, _probeDirection, CorridorLength);
    }
}