using LuckNGold.Generation;
using LuckNGold.Visuals.Screens;
using LuckNGold.Visuals.Windows;
using LuckNGold.World;
using LuckNGold.World.Decor.Wall;
using LuckNGold.World.Furniture;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Components;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold;

// The "RootScreen" represents the visible screen with a map and message log parts.
internal class RootScreen : ScreenObject
{
    public readonly GameMap Map;
    public readonly RogueLikeEntity Player;
    public readonly MessageLogConsole MessageLog;

    // TODO debug stuff to be deleted at some point
    readonly ScreenSurface _infoSurface;

    readonly InventoryWindow _inventoryWindow;

    public RootScreen()
    {
        // Generate a dungeon map
        Map = MapFactory.GenerateMap(GameMap.DefaultWidth, GameMap.DefaultHeight);
        Children.Add(Map);

        // Generate player and place them in first room of the main path
        Player = MonsterFactory.Player();
        var firstRoom = Map.Paths[0].FirstRoom;
        Player.Position = firstRoom.Area.Center;
        Map.AddEntity(Player);

        // sample decor
        var pos = (Player.Position.X, firstRoom.Area.Y - 1);
        var decor = new Flag(pos, "Red");
        Map.AddEntity(decor);

        // sample key
        var key = ItemFactory.BronzeKey();
        key.Position = (Player.Position.X, Player.Position.Y - 1);
        Map.AddEntity(key);

        key = ItemFactory.BronzeKey();
        key.Position = (Player.Position.X + 1, Player.Position.Y - 1);
        Map.AddEntity(key);

        // sample door
        var door = FurnitureFactory.Door(true, KeyColor.Bronze);
        door.Position = firstRoom.Connections.Find(c => c is Exit) is Exit exit ? 
            exit.Position : Point.None;
        Map.AddEntity(door);

        // Calculate initial FOV.
        Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

        // Center view on player as they move
        var followTargetComponent = new SurfaceComponentFollowTarget { Target = Player };
        Map.DefaultRenderer!.SadComponents.Add(followTargetComponent);

        // debug layer
        //var debug = new DebugSurface(Map);
        //debug.SadComponents.Add(followTargetComponent);
        //Children.Add(debug);

        // small static info screen to display information from debugging methods
        _infoSurface = new ScreenSurface(20, 10);
        Children.Add(_infoSurface);

        // Create a window to display player's inventory.
        var inventory = Player.AllComponents.GetFirstOrDefault<InventoryComponent>() ??
            throw new InvalidOperationException("Player is missing inventory.");
        _inventoryWindow = new InventoryWindow(inventory);
        int x = (Program.Width - _inventoryWindow.Width) / 2;
        int y = Program.Height - _inventoryWindow.Height - 1;
        _inventoryWindow.Position = (x, y);
        Children.Add(_inventoryWindow);

        // Add inventory keyboard shortcuts to map's keyboard controller
        var controller = Map.AllComponents.GetFirstOrDefault<KeybindingsComponent<GameMap>>() ??
            throw new InvalidOperationException("Map is missing keyboard controller.");
        controller.SetAction(Keys.D1, () => DropItem(0));
        controller.SetAction(Keys.D2, () => DropItem(1));
        controller.SetAction(Keys.D3, () => DropItem(2));
        controller.SetAction(Keys.D4, () => DropItem(3));
        controller.SetAction(Keys.D5, () => DropItem(4));
        controller.SetAction(Keys.D6, () => DropItem(5));
        controller.SetAction(Keys.D7, () => DropItem(6));
        controller.SetAction(Keys.D8, () => DropItem(7));
        controller.SetAction(Keys.D9, () => DropItem(8));
        controller.SetAction(Keys.D0, () => DropItem(9));

        // Create message log
        MessageLog = new MessageLogConsole(Program.Width, MessageLogConsole.DefaultHeight);
        //MessageLog.Position = new(0, Program.Height - MessageLogConsole.DefaultHeight);
        //Children.Add(MessageLog);
    }

    void DropItem(int index)
    {
        var inventory = Player.AllComponents.GetFirstOrDefault<InventoryComponent>() ?? 
            throw new InvalidOperationException("Player is missing inventory.");
        var item = _inventoryWindow.GetItem(index);
        if (item is not null)
            inventory.Drop(item);
    }
}