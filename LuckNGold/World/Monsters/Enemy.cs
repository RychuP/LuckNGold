using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters;

internal class Enemy : RogueLikeEntity
{
    public Enemy() : base(Color.Red, 'g', false, layer: (int)GameMap.Layer.Monsters)
    {
        // Add AI component to path toward player when in view.
        AllComponents.Add(new EnemyAI());
    }
}