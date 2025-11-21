using GoRogue.Random;
using LuckNGold.Primitives;
using LuckNGold.Resources;
using LuckNGold.World.Common.Components;
using LuckNGold.World.Common.Enums;
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

        // Add description component.
        var descriptionComponent = new DescriptionComponent(Strings.DoorDescription);
        door.AllComponents.Add(descriptionComponent);

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

            // Add lock description.
            var lockDescription = Strings.DoorLockDescription;
            string gemstone = $"#1{(GemstoneType)lockDifficulty}".ToLower();
            lockDescription = lockDescription.Replace("xxx", gemstone);
            descriptionComponent.StateDescription = lockDescription;

            // React to lock component removal.
            door.AllComponents.ComponentRemoved += (o, e) =>
            {
                if (e.Component is LockComponent)
                {
                    descriptionComponent.StateDescription = string.Empty;
                }
            };

            var lockComp = new LockComponent(lockDifficulty);
            door.AllComponents.Add(lockComp);
        }

        return door;
    }

    /// <summary>
    /// Gate like entity that can be operated remotely.
    /// </summary>
    public static RogueLikeEntity Gate(DoorOrientation orientation)
    {
        if (orientation == DoorOrientation.None)
            throw new ArgumentException("Gate requires an orientation.");

        // Get appearances.
        var glyphDef = Program.Font.GetGlyphDefinition($"ClosedGate");
        var closedAppearance = glyphDef.CreateColoredGlyph();
        var appearance = glyphDef.CreateColoredGlyph();
        glyphDef = Program.Font.GetGlyphDefinition($"OpenDoor{orientation}");
        var openAppearance = glyphDef.CreateColoredGlyph(10);

        // Create entity.
        var gate = new RogueLikeEntity(appearance, false, true, (int)GameMap.Layer.Furniture)
        {
            Name = "Gate"
        };

        var descriptionComponent = new DescriptionComponent(Strings.ClosedGateDescription);
        gate.AllComponents.Add(descriptionComponent);

        // Add actuator component to operate the gate.
        var actuatorComponent = new ActuatorComponent();
        gate.AllComponents.Add(actuatorComponent);

        // Add opening component.
        var openingComponent = new OpeningComponent();
        openingComponent.Closed += (o, e) =>
        {
            closedAppearance.CopyAppearanceTo(gate.AppearanceSingle!.Appearance);
            descriptionComponent.Description = Strings.ClosedGateDescription;
            gate.IsWalkable = false;
        };
        openingComponent.Opened += (o, e) =>
        {
            openAppearance.CopyAppearanceTo(gate.AppearanceSingle!.Appearance);
            descriptionComponent.Description = Strings.OpenGateDescription;
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
        var chestColor = GlobalRandom.DefaultRNG.NextBool() ? "Brown" : "Amber";

        string[] animations = [$"{chestColor}ClosedChest",
            $"{chestColor}EmptyOpenChest", $"{chestColor}FullOpenChest",
            $"{chestColor}EmptyChestOpening", $"{chestColor}FullChestOpening", 
            $"{chestColor}EmptyChestClosing", $"{chestColor}FullChestClosing"];
        var chest = new AnimatedRogueLikeEntity(animations, animations[0], false,
            GameMap.Layer.Furniture, false)
        {
            Name = $"{chestColor} Chest"
        };

        // Add description component.
        var description = chestColor == "Brown" ? Strings.BrownChestDescription :
            Strings.AmberChestDescription;
        var contentDescription = items.Count > 0 ? Strings.FullChestDescription :
            Strings.EmptyChestDescription;
        var descriptionComponent = new DescriptionComponent(description, contentDescription);
        chest.AllComponents.Add(descriptionComponent);

        // Add loot spawner
        var loot = new LootSpawnerComponent(items);
        chest.AllComponents.Add(loot);

        var prefix = items.Count > 0 ? $"{chestColor}Full" : $"{chestColor}Empty";

        // Add opening component
        var openingComponent = new OpeningComponent($"{prefix}OpenChest", animations[0],
            $"{prefix}ChestOpening", $"{prefix}ChestClosing");
        openingComponent.Opened += (o, e) =>
        {
            loot.DropItems();
        };
        chest.AllComponents.Add(openingComponent);

        // Add contents emptied handling
        loot.Emptied += (o, e) =>
        {
            openingComponent.OpenAnimation = animations[1];
            openingComponent.OpeningAnimation = animations[3];
            openingComponent.ClosingAnimation = animations[5];
            descriptionComponent.StateDescription = Strings.EmptyChestDescription;
        };

        return chest;
    }

    /// <summary>
    /// Lever like entity that can be turned left (off) and right (on).
    /// </summary>
    public static AnimatedRogueLikeEntity Lever()
    {
        string[] animations = ["LeverLeft", "LeverRight", 
            "LeverTurningLeft", "LeverTurningRight",
            "LeverAbortTurningLeft", "LeverAbortTurningRight"];
        var lever = new AnimatedRogueLikeEntity(animations, animations[0], false,
            GameMap.Layer.Furniture, false)
        {
            Name = "Lever"
        };

        // Add description component.
        var descriptionComponent = new DescriptionComponent(Strings.LeverDescription);
        lever.AllComponents.Add(descriptionComponent);

        // Add switch component.
        var switchComponent = new SwitchComponent(animations[1], animations[0],
            animations[3], animations[2], animations[5], animations[4]);
        lever.AllComponents.Add(switchComponent);

        return lever;
    }
}