using GoRogue.GameFramework;
using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Subclass of the integration library's keybindings component that handles player movement 
/// and other game world activities.
/// </summary>
// TODO some methods don't really belong here. Move them to more appropriate classes.
internal class CustomKeybindingsComponent : KeybindingsComponent
{
    readonly GameMap _map;
    readonly RogueLikeEntity _player;
    readonly QuickAccessComponent _quickAccess;

    readonly static IEnumerable<(InputKey binding, Direction direction)> ViMotions =
    [
        ((InputKey)Keys.K, Direction.Up),
        (Keys.L, Direction.Right),
        (Keys.J, Direction.Down),
        (Keys.H, Direction.Left),
        (Keys.U, Direction.UpRight),
        (Keys.N, Direction.DownRight),
        (Keys.B, Direction.DownLeft),
        (Keys.Y, Direction.UpLeft)
    ];

    readonly static Keys[] QuickAccessKeys = [Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4,
        Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9];

    public CustomKeybindingsComponent(GameMap map, RogueLikeEntity player)
    {
        _player = player;
        _map = map;

        _quickAccess = _player.AllComponents.GetFirst<QuickAccessComponent>();

        AddMapControls();
        AddPlayerControls();
    }

    void AddPlayerControls()
    {
        // Add player motions
        SetMotions(ViMotions);
        SetMotions(ArrowMotions);
        SetMotions(NumPadAllMotions);
        SetMotions(WasdMotions);

        // Add quick access actions
        foreach (var key in QuickAccessKeys)
        {
            AddDropItemAction(key);
            AddUseAction(key);
        }

        // Add pick up action
        SetAction(Keys.G, () => _quickAccess.PickUp());
        SetAction(Keys.F, Interact);
    }

    // Adds action that will use the item on pressing the given key
    void AddUseAction(Keys key)
    {
        int slotIndex = GetSlotIndex(key);
        SetAction(key, () => _quickAccess.Use(slotIndex));
    }
    
    // Adds action that will drop the item on pressing the given key with shift as modifier
    void AddDropItemAction(Keys key)
    {
        int slotIndex = GetSlotIndex(key);
        InputKey inputKey = new(key, KeyModifiers.Shift);
        SetAction(inputKey, () => _quickAccess.Drop(slotIndex));
    }

    // Converts shortcut keyboard key to 0 based slot index of the quick access
    static int GetSlotIndex(Keys key) =>
        key == Keys.D0 ? 9 : (int) key - 49;

    // Adds zoom in and out keyboard shortcuts
    void AddMapControls()
    {
        SetAction(Keys.C, _map.ZoomViewIn);
        SetAction(Keys.Z, _map.ZoomViewOut);
    }

    void Interact()
    {
        var neighbours = AdjacencyRule.EightWay.Neighbors(_player.Position);
        foreach (var point in neighbours)
        {
            var entities = _map.GetEntitiesAt<RogueLikeEntity>(point);
            foreach (var entity in entities)
            {
                if (entity.AllComponents.GetFirstOrDefault<IInteractable>()
                    is IInteractable interactable)
                {
                    if (interactable.Interact(_player))
                    {
                        return;
                    }
                }
            }
        }
    }

    // Motion handler for the player movement
    protected override void MotionHandler(Direction direction)
    {
        if (!_player.CanMoveIn(direction)) return;
        _player.Position += direction;
    }
}