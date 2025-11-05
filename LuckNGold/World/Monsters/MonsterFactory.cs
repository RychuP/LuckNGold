using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters;

static class MonsterFactory
{
    public static RogueLikeEntity Player()
    {
        var player = new RogueLikeEntity(251, false, layer: (int)GameMap.Layer.Monsters)
        {
            Name = "Player"
        };

        // Add component that holds identity information.
        player.AllComponents.Add(new IdentityComponent("Henry", Race.Human));

        // Add component that represents the equipment worn and carried.
        player.AllComponents.Add(new EquipmentComponent());

        ColoredGlyph[] appearances = [
            new(Color.White, Color.Transparent, 250),
            new(Color.White, Color.Transparent, 251),
            new(Color.White, Color.Transparent, 252),
            new(Color.White, Color.Transparent, 260),
            new(Color.White, Color.Transparent, 261),
            new(Color.White, Color.Transparent, 262),
            new(Color.White, Color.Transparent, 270),
            new(Color.White, Color.Transparent, 271),
            new(Color.White, Color.Transparent, 272),
            new(Color.White, Color.Transparent, 280),
            new(Color.White, Color.Transparent, 281),
            new(Color.White, Color.Transparent, 282),
        ];

        // Add component that updates entity appearance as it moves.
        player.AllComponents.Add(new MotionComponent(appearances));

        // Add inventory component.
        player.AllComponents.Add(new InventoryComponent(20));

        // Add wallet to hold coins.
        player.AllComponents.Add(new WalletComponent());

        // Add quick access component displayed at the bottom of the screen.
        player.AllComponents.Add(new QuickAccessComponent());

        // Add component for updating map's player FOV as they move.
        player.AllComponents.Add(new PlayerFOVController());

        return player;
    }
}