using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Overlays;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Interfaces;
using SadConsole.UI.Controls;

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

    // Window that displays an info about a pointer selected entity.
    readonly EntityInfoWindow _entityInfoWindow = new();

    // Layer that displays monster onion appearances.
    readonly MonsterLayer _monsterLayer;

    // Reference to the component added to the map, so that it can sometimes be called manually.
    readonly FollowTargetComponent _followTargetComponent;

    IEnumerable<GameScreenKeybindingsComponent> KeybindingsComponents
    {
        get
        {
            yield return _playerKeybindingsComponent;
            yield return _pointerKeybindingsComponent;
        }
    }

    /// <summary>
    /// Initializes an instance of <see cref="GameScreen"/> class with default parameters.
    /// </summary>
    public GameScreen()
    {
        // Generate the dungeon map.
        Map = GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight, 16);
        Map.ViewZoomChanged += Map_OnViewZoomChanged;
        Children.Add(Map);

        // Add monster layer above the map.
        _monsterLayer = new MonsterLayer(Map);
        Children.Add(_monsterLayer);

        // Create the player and place it in the first room of the main path.
        Player = GeneratePlayer();
        Map.AddEntity(Player);

        // Attach event handler to pointer.
        Pointer.PositionChanged += Pointer_OnPositionChanged;

        // Create keyboard handler components.
        _playerKeybindingsComponent = new(this);
        _pointerKeybindingsComponent = new(this);
        SadComponents.Add(_playerKeybindingsComponent);

        // Calculate initial FOV.
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Create a component that centers view on player as they move.
        _followTargetComponent = new FollowTargetComponent(Player);
        Map.DefaultRenderer!.SadComponents.Add(_followTargetComponent);
        _followTargetComponent.ViewChanged += FollowTargetComponent_OnViewChanged;

        // Debug screens with various testing info.
        AddDebugOverlays();

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

        // Add a window that displays information about the selected entity.
        Children.Add(_entityInfoWindow);
    }

    public bool IsShowingPopUpWindow()
    {
        return _entityInfoWindow.IsVisible;
    }

    public void ClosePopUpWindows()
    {
        HideEntityInfo();
    }

    public void ClosePopUpOrHidePointer()
    {
        if (IsShowingPopUpWindow())
            ClosePopUpWindows();
        else
            HidePointer();
    }

    public void UpdateKeybindings(ControlBase control)
    {
        foreach (var keybindingsComponent in KeybindingsComponents)
            keybindingsComponent.UpdateKeybindings(control);
    }

    void FollowTargetComponent_OnViewChanged(object? sender, EventArgs e)
    {
        var visibleMonsters = Map.Monsters
            .Where(m => m.IsVisible);

        // Update onion appearance position of all visible monsters.
        foreach (var monster in visibleMonsters)
        {
            if (monster.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
            {
                var viewPosition = Map.DefaultRenderer!.Surface.ViewPosition;
                onionComponent.CurrentFrame.Position = monster.Position - viewPosition;
            }
        }
    }
}