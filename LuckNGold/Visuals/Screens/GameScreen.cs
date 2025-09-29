using LuckNGold.Generation;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Decor.Wall;
using LuckNGold.World.Furniture;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Components;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed once the generation is complete.
/// It contains the map and various information windows.
/// </summary>
internal class GameScreen : ScreenObject
{
    public readonly GameMap Map;
    public readonly RogueLikeEntity Player;
    public readonly MessageLogConsole MessageLog;

    // TODO debug stuff to be deleted at some point
    readonly ScreenSurface _infoSurface;

    readonly QuickAccessWindow _quickAccessWindow;

    readonly KeybindingsComponent _keybindingsComponent;

    public GameScreen()
    {
        IsFocused = true;

        // Generate a dungeon map
        Map = MapFactory.GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Generate player and place them in first room of the main path
        Player = MonsterFactory.Player();
        var firstRoom = Map.Paths[0].FirstRoom;
        Player.Position = firstRoom.Area.Center;
        Map.AddEntity(Player);

        // Create keyboard handler
        _keybindingsComponent = new CustomKeybindingsComponent(Map, Player);
        SadComponents.Add(_keybindingsComponent);

        // sample decor
        var pos = (Player.Position.X, firstRoom.Area.Y - 1);
        var decor = new Flag(pos, "Red");
        Map.AddEntity(decor);

        // sample key
        var key = ItemFactory.BronzeKey();
        key.Position = (Player.Position.X, Player.Position.Y - 1);
        Map.AddEntity(key);

        key = ItemFactory.BronzeKey();
        key.Position = (Player.Position.X + 1, Player.Position.Y - 1);
        Map.AddEntity(key);

        // sample door
        var door = FurnitureFactory.Door(true, KeyColor.Bronze);
        door.Position = firstRoom.Connections.Find(c => c is Exit) is Exit exit ?
            exit.Position : Point.None;
        Map.AddEntity(door);

        // Calculate initial FOV
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

        // Create a window to display player's inventory
        var quickAccess = Player.AllComponents.GetFirstOrDefault<QuickAccessComponent>() ??
            throw new InvalidOperationException("Player is missing quick access component.");
        _quickAccessWindow = new QuickAccessWindow(quickAccess);
        int x = (Program.Width - _quickAccessWindow.Width) / 2;
        int y = Program.Height - _quickAccessWindow.Height - 1;
        _quickAccessWindow.Position = (x, y);
        Children.Add(_quickAccessWindow);

        // Create message log
        MessageLog = new MessageLogConsole(Program.Width, MessageLogConsole.DefaultHeight);
        //MessageLog.Position = new(0, Program.Height - MessageLogConsole.DefaultHeight);
        //Children.Add(MessageLog);
    }

    
}