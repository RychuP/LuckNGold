using LuckNGold.Generation;
using LuckNGold.World.Map;

namespace LuckNGold.Tests;

internal class DebugSurface : ScreenSurface
{
    ColoredGlyph DeadEndAppearance = new(Color.Red, Color.Transparent, 'X');
    ColoredGlyph ExitAppearance = new(Color.LightGreen, Color.Transparent, 'E');

    public DebugSurface(GameMap map) : base(map.Width, map.Height)
    {
        ViewWidth = Program.Width;
        ViewHeight = Program.Height;
        Surface.DefaultForeground = Color.CornflowerBlue;
        Surface.Clear();

        // draw debug information
        // draw trace of the main path
        DrawPath(map.Paths[0]);
        foreach (var path in map.Paths)
        {
            // draw connections of each room
            foreach (var room in path.Rooms)
                DrawConnections(room);
        }
    }

    public void DrawConnections(Room room)
    {
        foreach (var connection in room.Connections)
        {
            (int x, int y) = connection is Exit exit ? exit.Position :
                room.GetConnectionPoint(connection.Direction);
            var appearance = connection.GetType() == typeof(Exit) ?
                ExitAppearance : DeadEndAppearance;
            Surface.SetCellAppearance(x, y, appearance);
        }
    }

    public void DrawPath(RoomPath path)
    {
        var color = Program.RandomColor;
        int glyph = 176;

        // draw lines between each room
        if (path.Count > 1)
        {
            int startIndex = 0;
            int endIndex = 1;
            do
            {
                var startRoom = path.Rooms[startIndex];
                var endRoom = path.Rooms[endIndex];
                var startPoint = startRoom.Area.Center;
                var endPoint = endRoom.Area.Center;
                Surface.DrawLine(startPoint, endPoint, glyph, color);
                startIndex = endIndex;
                endIndex++;
            }
            while (path.Rooms.Count > endIndex);
        }

        // draw line between start room from the parent
        // and the first room of the path
        if (path.StartRoom != null)
        {
            var start = path.StartRoom.Area.Center;
            var end = path.Rooms[0].Area.Center;
            Surface.DrawLine(start, end, glyph, color);
        }
    }
}