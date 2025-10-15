using GoRogue.GameFramework;
using LuckNGold.Generation;
using LuckNGold.Tests;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Decor;
using LuckNGold.World.Furniture;
using LuckNGold.World.Furniture.Components;
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

    // Message log from the SadRogue template
    public readonly MessageLogConsole MessageLog;

    // Debug surface that can print various information
    public static ScreenSurface InfoSurface { get; }
        = new ScreenSurface(40, 50) { IsVisible = false };
    static int s_y = 0;

    public static DebugSurface? DebugSurface { get; set; }

    // Window that shows player's quick access inventory
    readonly QuickAccessWindow _quickAccessWindow;

    // Window that displays player health, wealth and other stats
    readonly StatusWindow _statusWindow;

    // Keyboard handler for the map and player
    readonly KeybindingsComponent _keybindingsComponent;

    public GameScreen()
    {
        IsFocused = true;

        // Generate a dungeon map
        Map = MapFactory.GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Add event handlers that update fov when some entities change their transparency
        AddDoorTransparencyChangeHandler();
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

        // Get an exit from the first room for the sample door
        if (firstRoom.Connections.Find(c => c is Exit) is not Exit exit)
            throw new Exception("First room needs to have a valid exit.");

        // Add sample chest
        var coins = new RogueLikeEntity[5];
        for (int i = 0; i < 5; i++)
            coins[i] = ItemFactory.Coin();

        var chest = FurnitureFactory.Chest(coins);
        chest.Position = Player.Position + Direction.Down;
        Player.PositionChanged += (o, e) =>
        {
            var opening = chest.AllComponents.GetFirst<OpeningComponent>();
            if (opening.IsOpen)
                opening.Close();
        };
        Map.AddEntity(chest);

        // Calculate initial FOV
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Center view on player as they move
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer!.SadComponents.Add(followTargetComponent);

        // Debug layer
        DebugSurface = new DebugSurface(Map) { IsVisible = false };
        DebugSurface.SadComponents.Add(followTargetComponent);
        Children.Add(DebugSurface);

        // Small static info screen to display information from debugging methods
        Children.Add(InfoSurface);

        // Create a window to display player's inventory
        var quickAccess = Player.AllComponents.GetFirst<QuickAccessComponent>();
        _quickAccessWindow = new QuickAccessWindow(quickAccess);
        int x = (Program.Width - _quickAccessWindow.Width) / 2;
        int y = Program.Height - _quickAccessWindow.Height - 1;
        _quickAccessWindow.Position = (x, y);
        Children.Add(_quickAccessWindow);

        // Create a window to display player status
        var wallet = Player.AllComponents.GetFirst<WalletComponent>();
        _statusWindow = new StatusWindow(wallet) { Position = (0, 1) };
        Children.Add(_statusWindow);

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

    // Removes event handler to entities that have the ability to change their transparency
    void Map_OnObjectRemoved(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is RogueLikeEntity entity && entity.Name == "Door")
            entity.TransparencyChanged -= RogueLikeEntity_OnTransparencyChanged;
    }

    // Adds transparency handler to doors added at the map factory stage
    void AddDoorTransparencyChangeHandler()
    {
        var doors = Map.Entities.Where(ip => ip.Item is RogueLikeEntity entity
            && entity.Name == "Door").Select(ip => ip.Item as RogueLikeEntity).ToList();
        if (doors.Count > 0)
        {
            foreach (var door in doors)
            {
                if (door is not null)
                    door.TransparencyChanged += RogueLikeEntity_OnTransparencyChanged;
            }
        }
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

    /// <summary>
    /// Prints consecutive lines on the debug info screen.
    /// </summary>
    public static void Print(string text)
    {
        if (text.Length > InfoSurface.Width)
            text = text[..InfoSurface.Width];
        InfoSurface.Surface.Print(0, s_y++, text);
    }
}