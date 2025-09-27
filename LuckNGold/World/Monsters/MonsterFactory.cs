using LuckNGold.World.Monsters.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.World.Monsters;

static class MonsterFactory
{
    public static RogueLikeEntity Player()
    {
        var player = new RogueLikeEntity(2, false, layer: (int)GameMap.Layer.Monsters);
        var inventory = new InventoryComponent(10);

        // Add component that handles controls
        var controller = new CustomKeybindingsComponent();
        controller.SetMotions(KeybindingsComponent.ArrowMotions);
        controller.SetMotions(KeybindingsComponent.NumPadAllMotions);
        controller.SetMotions(KeybindingsComponent.WasdMotions);
        controller.SetMotions(CustomKeybindingsComponent.ViMotions);
        controller.SetAction(Keys.G, () => inventory.PickUp());
        player.AllComponents.Add(controller);

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController());

        // Add component that handles inventory
        player.AllComponents.Add(inventory);

        player.Name = "Player";
        return player;
    }
}