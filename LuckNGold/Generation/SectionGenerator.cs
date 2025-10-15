using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation;

/// <summary>
/// Generates sections with locked doors and associated keys.
/// </summary>
/// <remarks>Section is a part of the dungeon between two 
/// progressively more difficult locked doors.</remarks>
internal class SectionGenerator() : GenerationStep("SectionGen",
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"),
    new ComponentTypeTagPair(typeof(ItemList<Room>), "Rooms"))
{
    /// <summary>
    /// Minimum number of side paths connected to the current selection of rooms in 
    /// the main path that is required in order to form one section of the dungeon
    /// </summary>
    const int MinNumberSidePathsPerSection = 2;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths");
        var rooms = context.GetFirst<ItemList<Room>>("Rooms");
        var doors = context.GetFirstOrNew(() => new ItemList<Door>(), "Doors");
        var sections = context.GetFirstOrNew(() => new ItemList<Section>(), "Sections");
        var mainPath = paths.Items[0];
        int gemstoneCount = Enum.GetNames(typeof(Difficulty)).Length - 1;
        int sectionsRequired = gemstoneCount;

        // Count how many sections can be made out of available side paths
        int sidePathTotalCount = paths.Where(e => e.Step == "SidePath").Count();
        double sidePathsPerSection = sidePathTotalCount / (double)sectionsRequired;

        // Reduce the number of sections on the main path if the side path count is too small
        if (sidePathsPerSection < MinNumberSidePathsPerSection)
        {
            while (sidePathsPerSection < MinNumberSidePathsPerSection)
            {
                sectionsRequired--;

                sidePathsPerSection = sidePathTotalCount / (double)sectionsRequired;
                if (sectionsRequired == 1)
                    break;
            }
        }

        GameScreen.Print($"Sections req: {sectionsRequired}");
        GameScreen.Print($"Side paths: {sidePathTotalCount}");
        GameScreen.Print($"Paths per sec: {sidePathsPerSection:0.00}");

        int roomIndex = 0;
        int sidePathCount = 0;
        int sectionCount = 0;
        int oneButLastRoomIndex = mainPath.Count - 2;
        double sidePathCountRequired = sidePathsPerSection;
        Gemstone currentGemstone = Gemstone.None;
        Room currentRoom = mainPath.FirstRoom;

        while (sectionCount < sectionsRequired)
        {
            // Create a new section;
            sectionCount++;
            currentGemstone++;
            Section currentSection = new(currentGemstone);
            sections.Add(currentSection, Name);

            if (sidePathCount < sidePathTotalCount)
            {
                while (sidePathCount < sidePathCountRequired)
                {
                    currentRoom = mainPath.Rooms[roomIndex];
                    currentSection.Add(currentRoom);

                    // Check room branches out to other paths.
                    if (currentRoom.IsStartRoom())
                        sidePathCount += currentRoom.SidePathExits.Count();

                    GameScreen.Print($"- Room: {roomIndex++}, Paths: {sidePathCount}");
                }
            }
            else 
            {
                while (roomIndex != oneButLastRoomIndex)
                {
                    currentRoom = mainPath.Rooms[roomIndex];
                    currentSection.Add(currentRoom);
                    GameScreen.Print($"- Room: {roomIndex++}, Paths: {sidePathCount}");
                }
            }

            var nextRoom = mainPath.Rooms[roomIndex];

            // Find an exit leading to the next room of the path.
            if (!currentRoom.TryGetExit(nextRoom, out Exit? exitToNextRoom))
                throw new MissingOrNotValidExitException();

            // Create the door leading to the next section and its key.
            var position = currentRoom.Area.Center;
            var key = new Key(currentGemstone, position);
            var @lock = new Lock((Difficulty)currentGemstone, key);
            var door = new Door(exitToNextRoom, @lock);
            doors.Add(door, Name);

            GameScreen.Print($"Paths: {sidePathCount}, " +
                $"Req: {sidePathCountRequired:0.00}");
            GameScreen.Print($"Section: {currentSection.Gemstone}, " +
                $"Rooms: {currentSection.Rooms.Count}");

            // Increment path requirement for the next section.
            sidePathCountRequired += sidePathsPerSection;

        }

        yield break;
    }
}