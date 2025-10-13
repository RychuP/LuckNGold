using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation;

internal class DoorGenerator() : GenerationStep("DoorGenerator",
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"))
{
    /// <summary>
    /// Minimum number of side paths connected to the current selection of rooms in 
    /// the main path that is required in order to form one section of the dungeon
    /// </summary>
    const int MinNumberSidePathsPerSection = 2;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var doors = context.GetFirstOrNew(() => new ItemList<Door>(), "Doors");
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths");
        var mainPath = paths.Items[0];
        int gemstoneCount = Enum.GetNames(typeof(Difficulty)).Length - 1;
        int sectionsRequired = gemstoneCount;

        // Count how many sections can be made out of available side paths
        int sidePathCount = paths.Where(e => e.Step == "SidePath").Count();
        double sidePathsPerSection = sidePathCount / (double)sectionsRequired;

        // Reduce the number of sections on the main path if the side path count is too small
        if (sidePathsPerSection < MinNumberSidePathsPerSection)
        {
            while (sidePathsPerSection < MinNumberSidePathsPerSection)
            {
                sectionsRequired--;

                sidePathsPerSection = sidePathCount / (double)sectionsRequired;
                if (sectionsRequired == 1)
                    break;
            }
        }

        GameScreen.Print($"Sections req: {sectionsRequired}");
        GameScreen.Print($"Side paths: {sidePathCount}");
        GameScreen.Print($"Paths per sec: {sidePathsPerSection:0.00}");

        Gemstone currentGemstone = Gemstone.Onyx;

        // Add the first room to the initial section
        mainPath.Rooms[0].Section = currentGemstone;

        int sectionCount = 0;
        sidePathCount = 0;
        double sidePathCountRequired = sidePathsPerSection;

        GameScreen.Print($"Room Count: {mainPath.Count}");

        // Iterate through all the rooms in the main path skipping the first and the last room
        for (int i = 1; i < mainPath.Count - 1; i++)
        {
            // Current room of the main path
            var room = mainPath.Rooms[i];

            // Find side paths that begin with the current room
            var sidePaths = paths.Where(e => e.Item.StartRoom == room).Select(e => e.Item);

            // Count the side paths connected to the room
            if (sidePaths.Any())
            {
                foreach (var sidePath in sidePaths)
                    sidePathCount++;
            }

            GameScreen.Print($"- Room: {i}, Paths: {sidePathCount}");

            // Check the requirements and add the section
            if (sidePathCount >= sidePathCountRequired)
            {
                sectionCount++;

                // Check section is last required and place the door at the last room
                if (sectionCount == sectionsRequired)
                {
                    i = mainPath.Count - 2;
                    room = mainPath.Rooms[i];
                }
                var nextRoom = mainPath.Rooms[i + 1];

                // Add door and key
                var exitToNextRoom = room.Connections.Find(c => c is Exit exit
                    && exit.End is not null && exit.End.Room == nextRoom) as Exit ??
                    throw new InvalidOperationException("Valid exit couldn't be found.");
                var position = room.Area.Center;
                var key = new Key(currentGemstone, position);
                var @lock = new Lock((Difficulty)currentGemstone, key);
                var door = new Door(exitToNextRoom, @lock);
                doors.Add(door, Name);

                GameScreen.Print($"Room: {i}, Paths: {sidePathCount}, " +
                    $"Req: {sidePathCountRequired:0.00}");

                if (sectionCount < sectionsRequired)
                {
                    currentGemstone += 1;
                    sidePathCountRequired += sidePathsPerSection;
                }
            }
        }

        yield break;
    }
}