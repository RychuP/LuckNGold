using LuckNGold.Config;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Consoles;
using LuckNGold.Visuals.Consoles.InfoBoxes;
using LuckNGold.Visuals.Overlays;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Main game screen that is displayed once the generation is complete.
/// It contains the map and various information windows. 
/// </summary>
partial class GameScreen : ScreenObject
{
    // Window that shows player's quick access inventory
    readonly QuickAccessSlots _quickAccessWindow;

    // Window that displays player health, wealth and other stats
    readonly StatusSurface _statusWindow;

    // Window that displays info about an entity selected by the pointer .
    readonly EntityInfoWindow _entityInfoWindow = new();

    // Layer that displays monster onion appearances.
    readonly MonsterLayer _monsterLayer;

    // Layer that displays damage notifications.
    readonly DamageNotificationsLayer _damageNotificationsLayer;

    // Component that keeps the screen centered on either player or pointer.
    readonly FollowTargetComponent _followTargetComponent;

    /// <summary>
    /// Manager that oversees passage of time and order of turns.
    /// </summary>
    public TurnManager TurnManager { get; }

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

        // Create damage notifications layer.
        _damageNotificationsLayer = new();
        Children.Add(_damageNotificationsLayer);

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
        if (GameSettings.DebugEnabled)
            AddDebugOverlays();

        // Create a window to display player's inventory.
        var quickAccess = Player.AllComponents.GetFirst<QuickAccessComponent>();
        _quickAccessWindow = new QuickAccessSlots(quickAccess);
        int x = (GameSettings.Width * GameSettings.FontSize.X - _quickAccessWindow.Width) / 2;
        int y = GameSettings.Height * GameSettings.FontSize.Y - 
            _quickAccessWindow.Height - GameSettings.FontSize.Y;
        _quickAccessWindow.Position = (x, y);
        Children.Add(_quickAccessWindow);

        // Create a window to display player status.
        var wallet = Player.AllComponents.GetFirst<WalletComponent>();
        _statusWindow = new StatusSurface();
        Children.Add(_statusWindow);

        // Add entity info window to Children.
        Children.Add(_entityInfoWindow);

        // Create character window.
        _characterWindow = new(this);
        Children.Add(_characterWindow);

        // Add visibility changed event handler to character window.
        _characterWindow.IsVisibleChanged += CharacterWindow_OnIsVisibleChanged;

        // Create turn manager and start turns.
        TurnManager = new(this);
        TurnManager.CurrentEntityChanged += TurnManager_OnCurrentEntityChanged;
        TurnManager.PassTime();

        // Create status info boxes.
        _statusWindow.Children.Add(new HealthBox(Player.AllComponents.GetFirst<IHealth>()));
        _statusWindow.Children.Add(new CoinCounter(Player.AllComponents.GetFirst<IWallet>()));
        _statusWindow.Children.Add(new ActionCost(TurnManager));
        _statusWindow.Children.Add(new SpeedBox());
        _statusWindow.Children.Add(new TurnCounter(TurnManager));
    }

    public bool IsPlayerTurn() =>
        TurnManager.CurrentEntity == Player;
    
    /// <summary>
    /// Updates keybindings based on the control from the settings page.
    /// </summary>
    /// <param name="control">Control from the settings screen.</param>
    public void UpdateKeybindings(ControlBase control)
    {
        _playerKeybindingsComponent.UpdateKeybindings(control);
        _pointerKeybindingsComponent.UpdateKeybindings(control);
    }

    /// <summary>
    /// Updates onion appearance position in the monster layer after the position change of the view.
    /// </summary>
    void FollowTargetComponent_OnViewChanged(object? sender, ValueChangedEventArgs<Rectangle> e)
    {
        var visibleMonsters = Map.Monsters
            .Where(m => m.IsVisible);

        // Update onion appearance position of all visible monsters.
        foreach (var monster in visibleMonsters)
        {
            if (monster.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
            {
                var viewPosition = Map.DefaultRenderer!.Surface.ViewPosition;
                var framePosition = monster.Position - viewPosition;

                if (!onionComponent.IsBumping)
                {
                    onionComponent.CurrentFrame.Position = framePosition;
                }
                else
                {
                    int x = onionComponent.CurrentFrame.FontSize.X * framePosition.X;
                    int y = onionComponent.CurrentFrame.FontSize.Y * framePosition.Y;
                    onionComponent.BumpPosition = (x, y);
                }
            }
        }

        // Check view size has not changed.
        if (e.NewValue.Size == e.OldValue.Size)
        {
            // Update damage notifications positions.
            var deltaChange = e.OldValue.Position - e.NewValue.Position;
            var direction = Direction.GetDirection(deltaChange);
            _damageNotificationsLayer.UpdateNotificationsPosition(direction);
        }
    }

    void TurnManager_OnCurrentEntityChanged(object? o, ValueChangedEventArgs<RogueLikeEntity?> e)
    {
        if (e.NewValue is RogueLikeEntity monster)
        {
            if (monster.AllComponents.GetFirstOrDefault<IEnemyAI>() is IEnemyAI enemyAI)
            {
                var timeTracker = monster.AllComponents.GetFirst<ITimeTracker>();

                while (timeTracker.Time > 0)
                {
                    var action = enemyAI.TakeTurn();
                    TurnManager.Add(action);
                }
            }
        }
        else
        {
            TurnManager.PassTime();
        }
    }
}