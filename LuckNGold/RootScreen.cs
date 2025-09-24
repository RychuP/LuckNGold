using LuckNGold.World;
using SadConsole.Components;
using SadRogue.Integration;

namespace LuckNGold;

// The "RootScreen" represents the visible screen with a map and message log parts.
internal class RootScreen : ScreenObject
{
    public readonly GameMap Map;
    public readonly RogueLikeEntity Player;
    public readonly MessageLogConsole MessageLog;
    ScreenSurface _infoSurface;

    public RootScreen()
    {
        // Generate a dungeon map
        Map = MapFactory.GenerateDungeonMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Generate player, add to map at a random walkable position, and calculate initial FOV
        Player = MapObjectFactory.Player();
        //Player.Position = GlobalRandom.DefaultRNG.RandomPosition(Map.WalkabilityView, true);
        Player.Position = Map.Paths[0].FirstRoom.Area.Center;
        Map.AddEntity(Player);
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        //Player.PositionChanged += Player_OnPositionChanged;

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

        //Children.Add(new TerrainDrawingTest());
        //Children.Add(new GridViewTests());
        //Children.Add(new Test());

    }

    private void Player_OnPositionChanged(object? sender, ValueChangedEventArgs<Point> e)
    {
        _infoSurface.Clear();
        
    }
}