using LuckNGold.Visuals;
using LuckNGold.World.Furniture.Components;
using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Furniture;

/// <summary>
/// Factory of entities that can be interacted with like doors, chests, levers, etc
/// </summary>
internal class FurnitureFactory
{
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
        var opening = new OpeningComponent();
        opening.Closed += (o, e) =>
        {
            closedAppearance.CopyAppearanceTo(door.AppearanceSingle!.Appearance);
            door.IsWalkable = false;
            door.IsTransparent = false;
        };
        opening.Opened += (o, e) =>
        {
            openAppearance.CopyAppearanceTo(door.AppearanceSingle!.Appearance);
            door.IsWalkable = true;
            door.IsTransparent = true;
        };
        door.AllComponents.Add(opening);

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

    public static AnimatedRogueLikeEntity Chest(params RogueLikeEntity[] items)
    {
        string[] animations = ["ClosedChest", "OpenChest", "ChestOpening", "ChestClosing"];
        var chest = new AnimatedRogueLikeEntity(animations, "ClosedChest", false,
            GameMap.Layer.Furniture, false)
        {
            Name = "Chest"
        };
        chest.Finished += (o, e) =>
        {
            if (chest.CurrentAnimation == "ChestOpening")
                chest.CurrentAnimation = "OpenChest";
            else if (chest.CurrentAnimation == "ChestClosing")
                chest.CurrentAnimation = "ClosedChest";
        };

        // Add loot spawner
        var loot = new LootSpawnerComponent(items);
        chest.AllComponents.Add(loot);

        // Add opening component
        var opening = new OpeningComponent("ChestOpening", "ChestClosing");
        opening.Opened += (o, e) =>
        {
            loot.DropItems();
        };
        chest.AllComponents.Add(opening);

        return chest;
    }
}