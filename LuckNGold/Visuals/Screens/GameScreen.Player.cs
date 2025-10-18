using LuckNGold.World.Monsters;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    public RogueLikeEntity Player { get; }

    RogueLikeEntity GeneratePlayer()
    {
        var player = MonsterFactory.Player();
        var firstRoom = Map.Paths[0].FirstRoom;
        player.Position = firstRoom.Area.Center;
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