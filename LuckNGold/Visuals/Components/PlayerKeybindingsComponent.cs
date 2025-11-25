using GoRogue.GameFramework;
using LuckNGold.Config;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Components.Interfaces;
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
        AddQuickAccessControls();
        AddInteractionControls();
        AddPointerControls();
    }

    // Keyboard shortcuts relating to player interaction with environment.
    void AddInteractionControls()
    {
        SetAction(Keybindings.PickUp, () => _quickAccess.PickUp());
        SetAction(Keybindings.Interact, Interact);
    }

    void Interact()
    {
        var neighbours = GameSettings.Adjacency.Neighbors(GameScreen.Player.Position);

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
        var destination = MotionTarget.Position + direction;

        // Try moving player in the given direction.
        if (MotionTarget.CanMove(destination))
        {
            MotionTarget.Position = destination;
        }

        // Check if there is a monster that can be bumped.
        else
        {
            var monster = GameScreen.Map.GetEntityAt<RogueLikeEntity>(destination);
            if (monster != null && monster.AllComponents.GetFirstOrDefault<IBumpable>()
                is IBumpable bumpable)
            {
                bumpable.OnBumped(MotionTarget);
            }
        }
    }
}