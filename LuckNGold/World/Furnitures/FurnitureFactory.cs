using LuckNGold.Generation.Furnitures;
using LuckNGold.Visuals;
using LuckNGold.World.Furnitures.Components;
using LuckNGold.World.Furnitures.Enums;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Furnitures;

/// <summary>
/// Factory of entities that can be interacted with like doors, chests, levers, etc
/// </summary>
internal class FurnitureFactory
{
    /// <summary>
    /// Door like entity that seperates one room from another.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static RogueLikeEntity Door(DoorOrientation orientation,
        bool locked = false, Difficulty lockDifficulty = Difficulty.None)
    {
        if (orientation == DoorOrientation.None)
            throw new ArgumentException("Door requires an orientation.");

        // Get appearances
        var glyphDef = Program.Font.GetGlyphDefinition($"ClosedDoor{orientation}");
        var closedAppearance = glyphDef.CreateColoredGlyph();
        var appearance = glyphDef.CreateColoredGlyph();
        glyphDef = Program.Font.GetGlyphDefinition($"OpenDoor{orientation}");
        var openAppearance = glyphDef.CreateColoredGlyph();

        // Create container
        var door = new RogueLikeEntity(appearance, !locked, !locked, (int)GameMap.Layer.Furniture)
        {
            Name = "Door"
        };

        // Add opening component
        var openingComponent = new OpeningComponent();
        openingComponent.Closed += (o, e) =>
        {
            closedAppearance.CopyAppearanceTo(door.AppearanceSingle!.Appearance);
            door.IsWalkable = false;
            door.IsTransparent = false;
        };
        openingComponent.Opened += (o, e) =>
        {
            openAppearance.CopyAppearanceTo(door.AppearanceSingle!.Appearance);
            door.IsWalkable = true;
            door.IsTransparent = true;
        };
        door.AllComponents.Add(openingComponent);

        // Add lock component if marked as locked
        if (locked)
        {
            if (lockDifficulty == Difficulty.None)
                throw new ArgumentException("Locked door requires a lock difficulty level.");

            var lockComp = new LockComponent(lockDifficulty);
            door.AllComponents.Add(lockComp);
        }

        return door;
    }

    /// <summary>
    /// Gate like entity that can be operated remotely.
    /// </summary>
    public static RogueLikeEntity RemoteGate(DoorOrientation orientation)
    {
        if (orientation == DoorOrientation.None)
            throw new ArgumentException("Gate requires an orientation.");

        // Get appearances.
        var glyphDef = Program.Font.GetGlyphDefinition($"ClosedRemoteGate");
        var closedAppearance = glyphDef.CreateColoredGlyph();
        var appearance = glyphDef.CreateColoredGlyph();
        glyphDef = Program.Font.GetGlyphDefinition($"OpenDoor{orientation}");
        var openAppearance = glyphDef.CreateColoredGlyph(98);

        // Create entity.
        var gate = new RogueLikeEntity(appearance, false, true, (int)GameMap.Layer.Furniture)
        {
            Name = "Remote Gate"
        };

        // Add signal receiver component.
        var signalReceiverComponent = new SignalReceiverComponent();
        gate.AllComponents.Add(signalReceiverComponent);

        // Add opening component.
        var openingComponent = new OpeningComponent();
        openingComponent.Closed += (o, e) =>
        {
            closedAppearance.CopyAppearanceTo(gate.AppearanceSingle!.Appearance);
            gate.IsWalkable = false;
        };
        openingComponent.Opened += (o, e) =>
        {
            openAppearance.CopyAppearanceTo(gate.AppearanceSingle!.Appearance);
            gate.IsWalkable = true;
        };
        gate.AllComponents.Add(openingComponent);

        return gate;
    }

    /// <summary>
    /// Chest like entity that drops loot when opened.
    /// </summary>
    /// <param name="items">Item entities that will drop as loot when chest is open.</param>
    public static AnimatedRogueLikeEntity Chest(List<RogueLikeEntity> items)
    {
        string[] animations = ["ClosedChest", "OpenChest", "ChestOpening", "ChestClosing"];
        var chest = new AnimatedRogueLikeEntity(animations, "ClosedChest", false,
            GameMap.Layer.Furniture, false)
        {
            Name = "Chest"
        };

        // Add loot spawner
        var loot = new LootSpawnerComponent(items);
        chest.AllComponents.Add(loot);

        // Add opening component
        var openingComponent = new OpeningComponent("OpenChest", "ClosedChest", 
            "ChestOpening", "ChestClosing");
        openingComponent.Opened += (o, e) =>
        {
            loot.DropItems();
        };
        chest.AllComponents.Add(openingComponent);

        return chest;
    }

    /// <summary>
    /// Lever like entity that can be switched on and off.
    /// </summary>
    public static AnimatedRogueLikeEntity Lever()
    {
        string[] animations = ["LeverOff", "LeverOn", "LeverTurningOff", "LeverTurningOn"];
        var lever = new AnimatedRogueLikeEntity(animations, "LeverOff", false,
            GameMap.Layer.Furniture, false)
        {
            Name = "Lever"
        };

        var switchComponent = new SwitchComponent("LeverOn", "LeverOff",
            "LeverTurningOn", "LeverTurningOff");
        lever.AllComponents.Add(switchComponent);

        return lever;
    }
}