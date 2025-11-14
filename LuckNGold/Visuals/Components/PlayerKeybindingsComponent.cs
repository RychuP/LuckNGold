using GoRogue.GameFramework;
using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Monsters.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Keybinds for player in the gameplay mode.
/// </summary>
internal class PlayerKeybindingsComponent : GameScreenKeybindingsComponent
{
    readonly QuickAccessComponent _quickAccess;

    public PlayerKeybindingsComponent(GameScreen gameScreen) 
        : base(gameScreen, gameScreen.Player)
    {
        _quickAccess = gameScreen.Player.AllComponents.GetFirst<QuickAccessComponent>();

        AddMapZoomControls();
        AddDebugControls();
        AddQuickAccessControls();
        AddInteractionControls();
        AddPointerControls();
    }

    // Keyboard shortcuts relating to the quick access window.
    void AddQuickAccessControls()
    {
        // Add quick access actions
        foreach (var key in QuickAccessKeys)
        {
            AddDropItemAction(key);
            AddUseAction(key);
        }
    }

    // Keyboard shortcuts relating to player interaction with environment.
    void AddInteractionControls()
    {
        SetAction(Keybindings.PickUp, () => _quickAccess.PickUp());
        SetAction(Keybindings.Interact, Interact);
    }

    // Adds action that will use the item on pressing the given key.
    void AddUseAction(Keys key)
    {
        int slotIndex = GetSlotIndex(key);
        SetAction(key, () => _quickAccess.Use(slotIndex));
    }

    // Adds action that will drop the item on pressing the given key with shift as modifier.
    void AddDropItemAction(Keys key)
    {
        int slotIndex = GetSlotIndex(key);
        InputKey inputKey = new(key, KeyModifiers.Shift);
        SetAction(inputKey, () => _quickAccess.Drop(slotIndex));
    }

    // Converts shortcut keyboard key to 0 based slot index of the quick access.
    static int GetSlotIndex(Keys key) =>
        key == Keys.D0 ? 9 : (int)key - 49;

    void Interact()
    {
        var neighbours = Program.Adjacency.Neighbors(GameScreen.Player.Position);

        // TODO: collect all interactable components and make the player choose one.
        foreach (var point in neighbours)
        {
            var entities = GameScreen.Map.GetEntitiesAt<RogueLikeEntity>(point);
            foreach (var entity in entities)
            {
                if (entity.AllComponents.GetFirstOrDefault<IInteractable>()
                    is IInteractable interactable)
                {
                    interactable.Interact(GameScreen.Player);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Keyboard shortcut that shows the pointer.
    /// </summary>
    void AddPointerControls()
    {
        SetAction(Keybindings.Look, GameScreen.ShowPointer);
    }

    // Motion handler for the player movement.
    protected override void MotionHandler(Direction direction)
    {
        if (MotionTarget.CanMoveIn(direction))
            MotionTarget.Position += direction;
    }
}