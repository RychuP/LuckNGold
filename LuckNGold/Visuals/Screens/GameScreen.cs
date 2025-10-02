using GoRogue.GameFramework;
using LuckNGold.Generation;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Decor;
using LuckNGold.World.Furniture;
using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Components;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives.SpatialMaps;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Screen that is displayed once the generation is complete.
/// It contains the map and various information windows. 
/// Actual gameplay happens here.
/// </summary>
internal class GameScreen : ScreenObject
{
    // Map that displays the game world
    public readonly GameMap Map;

    // Player entity
    public readonly RogueLikeEntity Player;

    // TODO probably not needed -> delete at some point ?
    // Message log from the SadRogue template
    public readonly MessageLogConsole MessageLog;

    // TODO debug stuff to be deleted at some point
    readonly ScreenSurface _infoSurface;

    // Window that shows player's quick access inventory
    readonly QuickAccessWindow _quickAccessWindow;

    // Keyboard handler for the map and player
    readonly KeybindingsComponent _keybindingsComponent;

    public GameScreen()
    {
        IsFocused = true;

        // Generate a dungeon map
        Map = MapFactory.GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Just in case, add event handlers after adding map to gamescreen
        Map.ObjectAdded += Map_OnObjectAdded;
        Map.ObjectRemoved += Map_OnObjectRemoved;

        // Create a player entity and place it in the first room of the main path
        Player = MonsterFactory.Player();
        var firstRoom = Map.Paths[0].FirstRoom;
        Player.Position = firstRoom.Area.Center;
        Map.AddEntity(Player);
        //Player.PositionChanged += Player_OnPositionChanged;

        // Add keyboard handler component
        _keybindingsComponent = new CustomKeybindingsComponent(Map, Player);
        SadComponents.Add(_keybindingsComponent);

        // Add sample decor
        var flag = DecorFactory.Flag(Gemstone.Onyx);
        flag.Position = (Player.Position.X, firstRoom.Area.Y - 1);
        Map.AddEntity(flag);

        // Add sample keys
        var key = ItemFactory.Key(Gemstone.Onyx);
        key.Position = (Player.Position.X, Player.Position.Y - 1);
        Map.AddEntity(key);
        key = ItemFactory.Key(Gemstone.Ruby);
        key.Position = (Player.Position.X + 1, Player.Position.Y - 1);
        Map.AddEntity(key);
        key = ItemFactory.Key(Gemstone.Emerald);
        key.Position = (Player.Position.X - 1, Player.Position.Y - 1);
        Map.AddEntity(key);

        // Add sample candles
        var topSideOfRoom = firstRoom.Area.PositionsOnSide(Direction.Up);
        var candle = DecorFactory.Candle(Size.Small);
        candle.Position = topSideOfRoom.First();
        Map.AddEntity(candle);
        candle = DecorFactory.Candle(Size.Small);
        candle.Position = topSideOfRoom.Last();
        Map.AddEntity(candle);

        // Add sample torches
        var torch = DecorFactory.Torch();
        torch.Position = flag.Position + Direction.Right;
        Map.AddEntity(torch);
        torch = DecorFactory.Torch();
        torch.Position = flag.Position + Direction.Left;
        Map.AddEntity(torch);

        // Add sample coin
        var coin = ItemFactory.Coin();
        coin.Position = Player.Position + Direction.DownLeft;
        Map.AddEntity(coin);

        // Get an exit from the first room for the sample door
        if (firstRoom.Connections.Find(c => c is Exit) is not Exit exit)
            throw new Exception("First room has to have a usable exit.");

        // Establish orientation of the door
        var direction = exit.Direction;
        DoorOrientation doorOrientation = DoorOrientation.None;
        if (direction.IsHorizontal())
        {
            doorOrientation = direction == Direction.Left ?
                DoorOrientation.Left : DoorOrientation.Right;
        }
        else
        {
            doorOrientation = direction == Direction.Up ?
                DoorOrientation.TopLeft : DoorOrientation.BottomLeft;
        }

        // Create the sample door
        var door = FurnitureFactory.Door(doorOrientation, true, Difficulty.Trivial);
        door.Position = exit.Position;
        Map.AddEntity(door);

        // Calculate initial FOV
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Center view on player as they move
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer!.SadComponents.Add(followTargetComponent);

        // Debug layer
        //var debug = new DebugSurface(Map);
        //debug.SadComponents.Add(followTargetComponent);
        //Children.Add(debug);

        // Small static info screen to display information from debugging methods
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

    // Adds event handler to entities that have the ability to change their transparency
    void Map_OnObjectAdded(object? o, ItemEventArgs<IGameObject> e)
    {
        // Look for any known entities that can change their transparency
        // so that fov can be recalculated if in view range of the player
        if (e.Item is RogueLikeEntity entity && entity.Name == "Door")
            entity.TransparencyChanged += RogueLikeEntity_OnTransparencyChanged;
    }

    void Map_OnObjectRemoved(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is RogueLikeEntity entity && entity.Name == "Door")
            entity.TransparencyChanged -= RogueLikeEntity_OnTransparencyChanged;
    }

    // Recalculates player FOV if an entity that changed its transparency is in view
    void RogueLikeEntity_OnTransparencyChanged(object? o, ValueChangedEventArgs<bool> e)
    {
        if (o is not RogueLikeEntity door || door.Name != "Door")
            return;

        var playerFOV = Player.AllComponents.GetFirst<PlayerFOVController>();
        if (Map.PlayerFOV.CurrentFOV.Contains(door.Position))
            playerFOV.CalculateFOV();
    }
}