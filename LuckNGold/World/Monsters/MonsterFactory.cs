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
        var inventory = new InventoryComponent(10);
        player.AllComponents.Add(inventory);

        // Add quick access component
        var quickAccess = new QuickAccessComponent(inventory);
        player.AllComponents.Add(quickAccess);

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController());

        return player;
    }
}