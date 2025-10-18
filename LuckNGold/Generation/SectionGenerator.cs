using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation.Decor;
using LuckNGold.Generation.Furniture;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation;

/// <summary>
/// Generates sections of the dungeon with locked doors and steps leading to other levels.
/// </summary>
internal class SectionGenerator() : GenerationStep("Sections",
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
        var sections = context.GetFirstOrNew(() => new ItemList<Section>(), "Sections");
        var decor = context.GetFirstOrNew(() => new ItemList<Entity>(), "Decor");
        var furniture = context.GetFirstOrNew(() => new ItemList<Entity>(), "Furniture");
        var items = context.GetFirstOrNew(() => new ItemList<Item>(), "Items");

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

        // Index of the main path room currently being checked and added to a section.
        int roomIndex = 0;

        // Number of side paths encountered so far.
        int sidePathCount = 0;

        // Number of sections created so far.
        int sectionCount = 0;

        // Index of the room where final locked door will be placed
        int oneButLastRoomIndex = mainPath.Count - 2;

        // Number of sections required before change of a section occurs.
        double sidePathCountRequired = sidePathsPerSection;

        // Gemstone type of the current section.
        Gemstone currentGemstone = Gemstone.None;

        // Room from the main path.
        Room currentRoom = mainPath.FirstRoom;

        // Create sections, locked doors in the section exit rooms and associated keys.
        while (sectionCount < sectionsRequired)
        {
            // Create a new section.
            sectionCount++;
            currentGemstone++;
            Section currentSection = new(currentGemstone);
            sections.Add(currentSection, Name);

            // Add rooms to the section.
            while (sidePathCount < sidePathCountRequired ||
                    (sidePathCount == sidePathTotalCount && roomIndex <= oneButLastRoomIndex))
            {
                currentRoom = mainPath.Rooms[roomIndex];
                currentSection.Add(currentRoom);

                // Check room branches out to other paths.
                if (currentRoom.IsStartRoom())
                    sidePathCount += currentRoom.SidePathExits.Count();

                GameScreen.Print($"- Room: {roomIndex++}, Paths: {sidePathCount}");
            }

            var nextRoom = mainPath.Rooms[roomIndex];

            // Find an exit leading to the next room of the path.
            if (!currentRoom.TryGetExit(nextRoom, out Exit? exitToNextRoom))
                throw new MissingOrNotValidExitException();

            // Create the door to the next section.
            var doorPosition = exitToNextRoom.Position;
            var isDouble = exitToNextRoom.IsDouble;
            var doorDirection = exitToNextRoom.Direction;
            var @lock = new Lock((Difficulty)currentGemstone);
            var door = new Door(doorPosition, doorDirection, isDouble, @lock);
            furniture.Add(door, Name);

            // Save current room as the exit from the section.
            currentSection.Exit = currentRoom;

            // Save distance to exit for each room.
            foreach (Room room in currentSection.Rooms)
                room.DistanceToSectionExit = currentSection.GetDistance(room);

            GameScreen.Print($"Paths: {sidePathCount}, " +
                $"Req: {sidePathCountRequired:0.00}");
            GameScreen.Print($"Section: {currentSection.Gemstone}, " +
                $"Rooms: {currentSection.Rooms.Count}");

            // Increment path requirement for the next section.
            if (sidePathCount < sidePathTotalCount)
                sidePathCountRequired += sidePathsPerSection;
        }

        // Create steps leading to a higher level.
        var firstRoom = mainPath.FirstRoom;
        var exit = firstRoom.Exits.FirstOrDefault() ??
            throw new InvalidOperationException("No valid exits in the first room.");
        var position = firstRoom.Area.PerimeterPositions().Where(p => p.Y == exit.Position.Y
            && Math.Abs(exit.Position.X - p.X) > 1).First();
        var stepsUp = new Steps(position, leadDown: false,
            faceRight: exit.Direction.GetOpposite() == Direction.Right);
        decor.Add(stepsUp, Name);

        // Create steps leading to a lower level.
        var lastRoom = mainPath.LastRoom;
        exit = lastRoom.Exits.FirstOrDefault() ??
            throw new InvalidOperationException("No valid exits in the last room.");
        position = lastRoom.Area.Center;
        var direction = exit.Direction.IsHorizontal() ?
            exit.Direction.GetOpposite() : Direction.Left;
        var stepsDown = new Steps(position, leadDown: true,
            faceRight: direction == Direction.Right);
        decor.Add(stepsDown, Name);

        yield break;
    }
}