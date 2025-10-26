using LuckNGold.Generation.Furnitures;
using LuckNGold.World.Common.Enums;
using LuckNGold.World.Furnitures;
using LuckNGold.World.Furnitures.Components;
using LuckNGold.World.Furnitures.Enums;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates furniture data objects from generator to RogueLike entities.
partial class GameMap
{
    public void PlaceFurniture(Furniture furniture)
    {
        if (furniture.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(furniture.Position);
        if (entity is not null)
        {
            // Go through objects that may be created by other objects.
            if (furniture is Gate gate && gate.IsOperateRemotely)
                return;
            else
                throw new InvalidOperationException("Another entity already at location.");
        }

        if (furniture is Door door)
            Place(door);
        else if (furniture is Chest chest)
            Place(chest);
        else if (furniture is Gate gate && !gate.IsOperateRemotely)
            Place(gate);
        else if (furniture is Lever lever)
            Place(lever);
    }

    RogueLikeEntity Place(Gate gateData)
    {
        var gate = FurnitureFactory.Gate(gateData.Orientation);
        AddEntity(gate, gateData.Position);
        return gate;
    }

    void Place(Lever leverData)
    {
        var lever = FurnitureFactory.Lever();
        var switchComponent = lever.AllComponents.GetFirst<ISwitch>();
        AddEntity(lever, leverData.Position);

        if (leverData.Target is Gate gateData)
        {
            var gate = Place(gateData);
            var actuatorComponent = gate.AllComponents.GetFirst<IActuator>();

            switchComponent.StateChanging += (o, e) =>
            {
                bool isTurningLeft = !e.NewValue;
                var entitiesAtGate = GetEntitiesAt<RogueLikeEntity>(gate.Position);
                bool gateIsBlocked = entitiesAtGate.Count() > 1;

                // Lever is about to be put into On state, but the gate is blocked,
                // so lever has to be stopped.
                if (isTurningLeft && gateIsBlocked)
                    switchComponent.StopStateChanging();
            };

            switchComponent.StateChanged += (o, e) =>
            {
                if (switchComponent.IsOn)
                    actuatorComponent.Extend();
                else
                    actuatorComponent.Retract();
            };
        }
    }

    void Place(Chest chestData)
    {
        var items = new List<RogueLikeEntity>(chestData.Items.Count);
        foreach (var item in chestData.Items)
            items.Add(CreateItem(item));

        var chest = FurnitureFactory.Chest(items);
        AddEntity(chest, chestData.Position);
    }

    void Place(Door doorData)
    {
        // Check if the door is locked.
        bool isLocked = false;
        Difficulty difficulty = Difficulty.None;
        if (doorData.Lock is Lock @lock)
        {
            isLocked = true;
            difficulty = @lock.Difficulty;
        }

        // Create the door.
        var door = FurnitureFactory.Door(doorData.Orientation, isLocked, difficulty);
        AddEntity(door, doorData.Position);

        // Add aditional door to wide corridors.
        if (doorData.IsDouble)
        {
            // Calculate door orientation.
            var doorOrientation = 
                doorData.Orientation == DoorOrientation.TopLeft ? DoorOrientation.TopRight : 
                doorData.Orientation == DoorOrientation.BottomLeft ? DoorOrientation.BottomRight :
                throw new InvalidOperationException("Double door can only be vertical.");

            // Create 2nd door.
            var door2 = FurnitureFactory.Door(doorOrientation, isLocked, difficulty);
            AddEntity(door2, door.Position + Direction.Right);

            // Pull out opening components.
            var openingComponent1 = door.AllComponents.GetFirst<OpeningComponent>();
            var openingComponent2 = door2.AllComponents.GetFirst<OpeningComponent>();

            // React to state changing and check for any entities blocking any of the doors.
            openingComponent1.StateChanging += (o, e) =>
            {
                bool isClosing = !e.NewValue;
                int entitiesCount = GetEntitiesAt<RogueLikeEntity>(door2.Position).Count();
                bool otherDoorIsBlocked = entitiesCount > 1;

                if (isClosing && otherDoorIsBlocked)
                    openingComponent1.StopStateChanging();
            };
            openingComponent2.StateChanging += (o, e) =>
            {
                bool isClosing = !e.NewValue;
                int entitiesCount = GetEntitiesAt<RogueLikeEntity>(door.Position).Count();
                bool otherDoorIsBlocked = entitiesCount > 1;

                if (isClosing && otherDoorIsBlocked)
                    openingComponent2.StopStateChanging();
            };

            // React to open/close action and mirror on both.
            openingComponent1.Opened += (o, e) => openingComponent2.Open();
            openingComponent1.Closed += (o, e) => openingComponent2.Close();
            openingComponent2.Opened += (o, e) => openingComponent1.Open();
            openingComponent2.Closed += (o, e) => openingComponent1.Close();

            // React to lock removal and mirror the action on both.
            door.AllComponents.ComponentRemoved += (o, e) =>
            {

                if (e.Component is LockComponent lockComp)
                {
                    var @lock = door2.AllComponents.GetFirstOrDefault<LockComponent>();
                    if (@lock != null)
                    {
                        var unlocker = new UnlockingComponent((Quality)lockComp.Difficulty);
                        @lock.Unlock(unlocker);
                    }
                }
            };
            door2.AllComponents.ComponentRemoved += (o, e) =>
            {

                if (e.Component is LockComponent lockComp)
                {
                    var @lock = door.AllComponents.GetFirstOrDefault<LockComponent>();
                    if (@lock != null)
                    {
                        var unlocker = new UnlockingComponent((Quality)lockComp.Difficulty);
                        @lock.Unlock(unlocker);
                    }
                }
            };
        }
    }
}