using LuckNGold.Config;
using LuckNGold.Generation.Map;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;

namespace LuckNGold.Visuals.Overlays;

/// <summary>
/// Overlay screen that shows visual layout of the map with all rooms, paths
/// and sections drawn. Used for debugging.
/// </summary>
internal class MapLayout : ScreenSurface
{
    const int SidePathGlyph = 176;
    const int MainPathGlyph = 249;

    readonly ColoredGlyph DeadEndAppearance = new(Color.Red, Color.Transparent, 'X');
    readonly ColoredGlyph ExitAppearance = new(Color.LightGreen, Color.Transparent, 'E');

    public MapLayout() : base(GameSettings.Width, GameSettings.Height)
    {
        IsVisible = false;
    }

    public void DrawOverlay(GameMap map)
    {
        // Resize to the map size first.
        if (Width != map.Width || Height != map.Height)
        {
            (Surface as ICellSurfaceResize)!.Resize(map.Width, map.Height, false);
            ViewWidth = GameSettings.Width;
            ViewHeight = GameSettings.Height;
        }
        
        //Surface.DefaultForeground = Color.CornflowerBlue;
        Surface.Clear();

        // Draw connections and section colors of each room.
        foreach (var path in map.Paths)
        {
            DrawPath(path);
            foreach (var room in path.Rooms)
            {
                DrawRoom(room);
                DrawConnections(room);
            }
        }

        // Draw distances of each room to the section exit.
        foreach (var section in map.Sections)
        {
            foreach (var room in section.Rooms)
            {
                int dist = room.DistanceToSectionExit;
                (int x, int y) = room.Position;
                Surface.Print(x, y, $"{dist}");
            }
        }
    }

    void DrawRoom(Room room)
    {
        var color = Theme.GemstoneColors[room.Section?.GemstoneType ?? GemstoneType.None];
        var shapeParams = ShapeParameters.CreateStyledBoxThin(color);
        Surface.DrawBox(room.Bounds, shapeParams);
    }

    void DrawConnections(Room room)
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
        var color = path.Name == "MainPath" ? Color.LightBlue : Program.RandomBrightColor;
        int glyph = path.Name == "MainPath" ? MainPathGlyph : SidePathGlyph;

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
            var startRoom = path.StartRoom;
            var endRoom = path.Rooms[0];
            if (startRoom.TryGetExit(endRoom, out var exit))
            {
                var startPoint = startRoom.Area.Center + exit.Direction;
                var endPoint = endRoom.Area.Center;
                Surface.DrawLine(startPoint, endPoint, glyph, color);
            }
        }
    }
}