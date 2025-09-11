using GoRogue.Random;
using SadConsole.Components;
using SadRogue.Integration;
using ShaiRandom.Generators;

namespace LuckNGold;

// The "RootScreen" represents the visible screen with a map and message log parts.
internal class RootScreen : ScreenObject
{
    public readonly GameMap Map;
    public readonly RogueLikeEntity Player;
    public readonly MessageLogConsole MessageLog;

    public RootScreen()
    {
        // Generate a dungeon map
        Map = MapFactory.GenerateDungeonMap(GameMap.DefaultWidth, GameMap.DefaultHeight);

        // Create a renderer for the map, specifying viewport size. The viewport size sets the visible width and
        // height of the renderer. The value in DefaultRenderer is automatically managed by the map, and renders
        // whenever the map is the active screen.
        //
        // CUSTOMIZATION: Pass in custom fonts/viewport sizes here.
        //
        // CUSTOMIZATION: If you want multiple renderers to render the same map, you can call CreateRenderer and
        // manage them yourself; but you must call the map's RemoveRenderer when you're done with these renderers,
        // and you must add any non-default renderers to the SadConsole screen object hierarchy, IN ADDITION
        // to the map itself.
        int viewHeight = Program.Height - MessageLogConsole.DefaultHeight;
        Point viewSize = new(Program.Width, viewHeight);
        Map.DefaultRenderer = Map.CreateRenderer(viewSize);

        // Make the Map (which is also a screen object) a child of this screen.  You MUST have the map as a child
        // of the active screen, even if you are using entirely custom renderers.
        //Children.Add(Map);

        // Make sure the map is focused so that it and the entities can receive keyboard input
        //Map.IsFocused = true;

        // Generate player, add to map at a random walkable position, and calculate initial FOV
        Player = MapObjectFactory.Player();
        Player.Position = GlobalRandom.DefaultRNG.RandomPosition(Map.WalkabilityView, true);
        Map.AddEntity(Player);
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Center view on player as they move
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer.SadComponents.Add(followTargetComponent);

        // Create message log
        MessageLog = new MessageLogConsole(Program.Width, MessageLogConsole.DefaultHeight);
        MessageLog.Position = new(0, Program.Height - MessageLogConsole.DefaultHeight);
        //Children.Add(MessageLog);

        Children.Add(new PixelFontTest());
    }
}