using LuckNGold.Visuals.Components;
using LuckNGold.World.Monsters;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    public RogueLikeEntity Player { get; }

    // Keyboard handler for the map and player.
    readonly PlayerKeybindingsComponent _playerKeybindingsComponent;

    RogueLikeEntity GeneratePlayer()
    {
        // Create player.
        var player = MonsterFactory.Player();
        var stepsUp = Map.Entities
            .Where(el => el.Item is RogueLikeEntity e && e.Name.StartsWith("StepsUp"))
            .Select(el => el.Item as RogueLikeEntity)
            .First() ?? throw new InvalidOperationException("Missing steps up.");
        var delta = stepsUp.Name.EndsWith("Left") ? Direction.Right : Direction.Left;
        player.Position = stepsUp.Position + delta;
        player.PositionChanged += Player_OnPositionChanged;
        return player;
    }

    void Player_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        // Close all openings that need to be closed.
        foreach (var opening in _openings)
        {
            if (opening.IsOpen)
                opening.Close();
        }
    }
}