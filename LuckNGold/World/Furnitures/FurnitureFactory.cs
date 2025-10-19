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
    /// Entity that seperates one room from another.
    /// </summary>
    /// <param name="orientation"></param>
    /// <param name="locked"></param>
    /// <param name="lockDifficulty"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static RogueLikeEntity Door(DoorOrientation orientation,
        bool locked = false, Difficulty lockDifficulty = Difficulty.None)
    {
        if (orientation == DoorOrientation.None)
            throw new ArgumentException("Door requires an orientation.", nameof(orientation));

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
    /// Entity that drops loot when opened.
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

        //chest.Finished += (o, e) =>
        //{
        //    if (chest.CurrentAnimation == "ChestOpening")
        //        chest.CurrentAnimation = "OpenChest";
        //    else if (chest.CurrentAnimation == "ChestClosing")
        //        chest.CurrentAnimation = "ClosedChest";
        //};

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