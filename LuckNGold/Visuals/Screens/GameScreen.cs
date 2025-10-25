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
        Map.AddEntity(Player);

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
       
    }
}