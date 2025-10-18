using LuckNGold.Generation.Furniture;
using LuckNGold.Generation.Map;
using LuckNGold.World.Furniture;
using LuckNGold.World.Furniture.Components;
using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

partial class GameMap
{
    public void PlaceFurniture(IReadOnlyList<Entity> furniture)
    {
        foreach (var entity in furniture)
        {
            if (entity.Position == Point.None)
                throw new InvalidOperationException("All entities in the list should have " +
                    "a valid position.");

            var temp = GetEntityAt<RogueLikeEntity>(entity.Position);
            if (temp is not null && !temp.IsWalkable)
                throw new InvalidOperationException("Non walkable entity already at location.");

            if (entity is Door door)
                Place(door);
            else if (entity is Chest chest)
                Place(chest);
        }
    }

    void Place(Chest data)
    {
        var items = new List<RogueLikeEntity>(data.Items.Count);
        foreach (var item in data.Items)
            items.Add(GetItem(item));

        var chest = FurnitureFactory.Chest(items);
        AddEntity(chest, data.Position);
    }

    void Place(Door data)
    {
        // Establish orientation of the door
        DoorOrientation doorOrientation;
        if (data.Direction.IsHorizontal())
        {
            doorOrientation = data.Direction == Direction.Left ?
                DoorOrientation.Left : DoorOrientation.Right;
        }
        else
        {
            doorOrientation = data.Direction == Direction.Up ?
                DoorOrientation.TopLeft : DoorOrientation.BottomLeft;
        }

        // Check if the door is locked.
        bool isLocked = false;
        Difficulty difficulty = Difficulty.None;
        if (data.Lock is Lock @lock)
        {
            isLocked = true;
            difficulty = @lock.Difficulty;
        }

        // Create the door.
        var door = FurnitureFactory.Door(doorOrientation, isLocked, difficulty);
        AddEntity(door, data.Position);

        // Add aditional door to wide corridors.
        if (data.IsDouble)
        {
            if (!data.Direction.IsVertical())
                throw new InvalidOperationException("Double door can only be vertical.");

            doorOrientation = data.Direction == Direction.Up ?
                DoorOrientation.TopRight : DoorOrientation.BottomRight;
            var door2 = FurnitureFactory.Door(doorOrientation, isLocked, difficulty);
            AddEntity(door2, door.Position + Direction.Right);

            // Add mirror behaviours to both doors
            door.AllComponents.GetFirst<OpeningComponent>().Opened +=
                (o, e) => door2.AllComponents.GetFirst<OpeningComponent>().Open();
            door.AllComponents.GetFirst<OpeningComponent>().Closed +=
                (o, e) => door2.AllComponents.GetFirst<OpeningComponent>().Close();
            door2.AllComponents.GetFirst<OpeningComponent>().Opened +=
                (o, e) => door.AllComponents.GetFirst<OpeningComponent>().Open();
            door2.AllComponents.GetFirst<OpeningComponent>().Closed +=
                (o, e) => door.AllComponents.GetFirst<OpeningComponent>().Close();
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