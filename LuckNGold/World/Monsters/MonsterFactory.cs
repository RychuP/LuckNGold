using LuckNGold.World.Items.Components;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.World.Monsters;

static class MonsterFactory
{
    public static RogueLikeEntity Player()
    {
        var player = new RogueLikeEntity(2, false, layer: (int)GameMap.Layer.Monsters)
        {
            Name = "Player"
        };

        // Add inventory component
        player.AllComponents.Add(new InventoryComponent(20));

        // Add wallet to hold coins
        player.AllComponents.Add(new WalletComponent());

        // Add quick access component displayed at the bottom of the screen
        player.AllComponents.Add(new QuickAccessComponent());

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController());

        // Add component that represents the outfit of the player entity
        player.AllComponents.Add(new OutfitComponent());

        // Add component that represents gear carried in hands by the player entity
        player.AllComponents.Add(new GearComponent());

        return player;
    }
}