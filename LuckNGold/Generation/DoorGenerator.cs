using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation;

internal class DoorGenerator() : GenerationStep("DoorGenerator",
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"))
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var doors = context.GetFirstOrNew(() => new ItemList<Door>(), "Doors");
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths");
        var mainPath = paths.Items[0];

        // Remove one difficulty level so that the first one can be used to lock
        // minor doors around the first section of the dungeon.
        // Door with a lock with the second difficulty level will lead to the next section.
        int lockCount = Enum.GetNames(typeof(Difficulty)).Length - 2;
        if (lockCount <= 0)
            throw new InvalidOperationException("Not enough lock difficulty levels.");

        // Plan is to make the highest difficulty lock open the last door of the main path
        // which holds the steps to the next level.
        var roomCount = mainPath.Count - 1;
        var doorStep = roomCount / (double)lockCount;

        for (int i = 1; i <= lockCount; i++)
        {
            int roomIndex = (int)Math.Round(i * doorStep) - 1;
            var currentRoom = mainPath.Rooms[roomIndex];
            var nextRoom = mainPath.Rooms[roomIndex + 1];

            var exitToNextRoom = currentRoom.Connections.Find(c => c is Exit exit &&
                exit.End is Exit destinationExit && destinationExit.Room == nextRoom) as Exit ?? 
                throw new InvalidOperationException("Exit to the next room couldn't be found.");

            var position = currentRoom.Area.Center;
            var key = new Key((Gemstone)i + 1, position);
            var @lock = new Lock((Difficulty)i + 1, key);
            var door = new Door(exitToNextRoom, @lock);
            doors.Add(door, Name);
        }

        yield break;
    }
}