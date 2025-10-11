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
        var inventory = new InventoryComponent(20);
        player.AllComponents.Add(inventory);

        // Add quick access component displayed at the bottom of the screen
        var quickAccess = new QuickAccessComponent();
        player.AllComponents.Add(quickAccess);

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController());

        // Add component that represents the outfit of the player entity
        player.AllComponents.Add(new OutfitComponent());

        // Add component that represents gear carried in hands by the player entity
        player.AllComponents.Add(new GearComponent());

        return player;
    }
}