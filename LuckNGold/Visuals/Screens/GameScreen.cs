using GoRogue.GameFramework;
using LuckNGold.Generation.Map;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Decors;
using LuckNGold.World.Furnitures;
using LuckNGold.World.Furnitures.Enums;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Main game screen that is displayed once the generation is complete.
/// It contains the map and various information windows. 
/// </summary>
partial class GameScreen : ScreenObject
{
    // Window that shows player's quick access inventory
    readonly QuickAccessWindow _quickAccessWindow;

    // Window that displays player health, wealth and other stats
    readonly StatusWindow _statusWindow;

    // Keyboard handler for the map and player
    readonly KeybindingsComponent _keybindingsComponent;

    /// <summary>
    /// Initializes an instance of <see cref="GameScreen"/> class with default parameters.
    /// </summary>
    public GameScreen()
    {
        IsFocused = true;

        // Generate the dungeon map.
        Map = GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight, 16);
        Children.Add(Map);

        // Create the player and place it in the first room of the main path.
        Player = GeneratePlayer();
        var entities = Map.GetEntitiesAt<RogueLikeEntity>(Player.Position);
        if (entities.Any())
        {
            foreach (var entity in entities)
            {
                if (entity.CanMoveIn(Direction.Up))
                {
                    entity.Position += Direction.Up;
                }
                else
                    Map.RemoveEntity(entity);
            }
        }
        Map.AddEntity(Player);

        // Sample entities for testing.
        AddSampleEntities();

        // Add keyboard handler component.
        _keybindingsComponent = new CustomKeybindingsComponent(Map, Player);
        SadComponents.Add(_keybindingsComponent);

        // Calculate initial FOV.
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Create a component that centers view on player as they move.
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer!.SadComponents.Add(followTargetComponent);

        // Debug screens with various testing info.
        AddDebugOverlays(followTargetComponent);

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
    }

    void AddSampleEntities()
    {
        var firstRoom = Map.Paths[0].FirstRoom;

        // Add sample candles
        var candle = DecorFactory.Candle(Size.Small);
        candle.Position = firstRoom.Area.MinYPositions().First();
        Map.AddEntity(candle);
        candle = DecorFactory.Candle(Size.Small);
        candle.Position = firstRoom.Area.MinYPositions().Last();
        Map.AddEntity(candle);

        // Get an exit from the first room for the sample door
        if (firstRoom.Exits.FirstOrDefault() is not Exit exit)
            throw new Exception("First room needs to have a valid exit.");

        // Add sample chest
        var coins = new List<RogueLikeEntity>(5);
        for (int i = 0; i < 5; i++)
            coins.Add(ItemFactory.Coin());
        var chest = FurnitureFactory.Chest(coins);
        chest.Position = firstRoom.Area.MaxYPositions().First();
        Map.AddEntity(chest);

        // Add sample gate.
        DoorOrientation gateOrientation = exit.Direction == Direction.Left ?
            DoorOrientation.Left : DoorOrientation.Right;
        var gate = FurnitureFactory.Gate(gateOrientation);
        var actuatorComponent = gate.AllComponents.GetFirst<IActuator>();
        gate.Position = exit.Position;
        Map.AddEntity(gate);

        // Add sample lever.
        var lever = FurnitureFactory.Lever();
        lever.Position = firstRoom.Area.MaxYPositions().Last();
        var switchComponent = lever.AllComponents.GetFirst<ISwitch>();
        switchComponent.StateChanged += (o, e) =>
        {
            if (switchComponent.IsOn)
                actuatorComponent.Extend();
            else
                actuatorComponent.Retract();
        };
        Map.AddEntity(lever);
    }
}