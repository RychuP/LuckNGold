using LuckNGold.Generation;
using LuckNGold.Visuals.Screens;
using LuckNGold.World;
using LuckNGold.World.Decor.Wall;
using LuckNGold.World.Furniture;
using LuckNGold.World.Items;
using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Components;
using SadConsole.Components;
using SadRogue.Integration;

namespace LuckNGold;

// The "RootScreen" represents the visible screen with a map and message log parts.
internal class RootScreen : ScreenObject
{
    public readonly GameMap Map;
    public readonly RogueLikeEntity Player;
    public readonly MessageLogConsole MessageLog;

    // TODO debug stuff to be deleted at some point
    readonly ScreenSurface _infoSurface;

    public RootScreen()
    {
        // Generate a dungeon map
        Map = MapFactory.GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Generate player and place them in first room of the main path
        Player = new Player();
        var firstRoom = Map.Paths[0].FirstRoom;
        Player.Position = firstRoom.Area.Center;
        Map.AddEntity(Player);
        
        //Player.PositionChanged += Player_OnPositionChanged;

        // sample decor
        var pos = (Player.Position.X, firstRoom.Area.Y - 1);
        var decor = new Flag(pos, "Red");
        Map.AddEntity(decor);

        // sample key
        pos = (Player.Position.X, Player.Position.Y - 1);
        var key = new Key(pos, "Silver");
        Map.AddEntity(key);

        // sample door
        if (firstRoom.Connections.Find(c => c is Exit) is Exit exit)
        {
            pos = exit.Position;
            var definition = Program.Font.GetGlyphDefinition("Door");
            var closed = definition.CreateColoredGlyph();
            var door = new Door(pos, closed, closed)
            {
                IsWalkable = true,
                IsTransparent = true,
            };
            Map.AddEntity(door);
        }

        // Calculate initial FOV.
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Center view on player as they move
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer!.SadComponents.Add(followTargetComponent);

        // debug layer
        //var debug = new DebugSurface(Map);
        //debug.SadComponents.Add(followTargetComponent);
        //Children.Add(debug);

        // small static info screen to display information from debugging methods
        _infoSurface = new ScreenSurface(20, 10);
        Children.Add(_infoSurface);

        // Create message log
        MessageLog = new MessageLogConsole(Program.Width, MessageLogConsole.DefaultHeight);
        //MessageLog.Position = new(0, Program.Height - MessageLogConsole.DefaultHeight);
        //Children.Add(MessageLog);
    }

    private void Player_OnPositionChanged(object? sender, ValueChangedEventArgs<Point> e)
    {
        _infoSurface.Clear();
        
    }
}