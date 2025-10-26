using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Map;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Common.Enums;
using LuckNGold.World.Items.Enums;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that creates sections of the dungeon with locked doors, steps leading to other levels
/// and section flags placed in most of the rooms.
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

    static readonly IEnhancedRandom _rnd = GlobalRandom.DefaultRNG;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths");
        var rooms = context.GetFirst<ItemList<Room>>("Rooms");
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
                if (currentRoom.IsPathStartRoom())
                    sidePathCount += currentRoom.SidePathExits.Count();

                GameScreen.Print($"- Room: {roomIndex++}, Paths: {sidePathCount}");
            }

            var nextRoom = mainPath.Rooms[roomIndex];

            // Find an exit leading to the next room of the path.
            if (!currentRoom.TryGetExit(nextRoom, out Exit? exitToNextRoom))
                throw new MissingOrNotValidExitException();

            CreateLockedDoor(exitToNextRoom, currentGemstone);
            CreateSectionFlags(currentSection);

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

        CreateStepsUp(mainPath.FirstRoom);
        CreateStepsDown(mainPath.LastRoom);       

        yield break;
    }

    /// <summary>
    /// Creates flags with a <see cref="Gemstone"/> color placed in <see cref="Section"/> rooms.
    /// </summary>
    static void CreateSectionFlags(Section section)
    {
        foreach (var room in section.Rooms)
        {
            // Give it 50% chance to place flags in the room.
            if (_rnd.NextBool())
                continue;

            // Skip placing flags in small, even sized rooms.
            if (room.Width == 4)
                continue;

            // Check if there is an exit in the given direction.
            if (room.TryGetExit(Direction.Up, out Exit? exit))
            {
                // Room is small enough. Is there a point to clog space with flags? Maybe..
                if (room.Width == 3)
                    continue;

                CreateTwoEvenlySpacedFlags(room);
            }
            else
            {
                if (room.Width.IsOdd() && (room.Width == 3 || _rnd.NextBool()))
                    CreateOneCenteredFlag(room);
                else
                    CreateTwoEvenlySpacedFlags(room);
            }
        }
    }

    static void CreateOneCenteredFlag(Room room)
    {
        var wallCenter = room.GetConnectionPoint(Direction.Up);
        CreateFlag(wallCenter, room);
    }

    static void CreateTwoEvenlySpacedFlags(Room room)
    {
        var wallCenter = room.GetConnectionPoint(Direction.Up);
        var deltaX = room.Width / 2;
        deltaX = _rnd.NextInt(1, deltaX);
        var flagPosition = wallCenter - (deltaX, 0);
        CreateFlag(flagPosition, room);
        flagPosition = wallCenter + (deltaX, 0);
        if (room.Width.IsEven())
            flagPosition += Direction.Right;
        CreateFlag(flagPosition, room);
    }

    static void CreateFlag(Point position, Room room)
    {
        var flag = new Flag(position, room.Section!.Gemstone);
        room.AddEntity(flag);
    }

    /// <summary>
    /// Creates locked door leading to the next dungeon <see cref="Section"/>.
    /// </summary>
    /// <param name="exit"><see cref="Exit"/> where door will be placed.</param>
    /// <param name="gemstone"><see cref="Gemstone"/> of the curent section.</param>
    static void CreateLockedDoor(Exit exit, Gemstone gemstone)
    {
        var doorPosition = exit.Position;
        var isDouble = exit.IsDouble;
        var doorDirection = exit.Direction;
        var @lock = new Lock((Difficulty)gemstone);
        var door = new Door(doorPosition, doorDirection, isDouble, @lock);
        exit.Room.AddEntity(door);
    }

    /// <summary>
    /// Creates <see cref="Steps"/> leading to the previous level.
    /// </summary>
    /// <param name="room">Room where steps will be placed.</param>
    /// <exception cref="InvalidOperationException"></exception>
    static void CreateStepsUp(Room room)
    {
        var exit = room.Exits.FirstOrDefault() ??
            throw new InvalidOperationException("No valid exits in the first room.");
        var position = room.Area.PerimeterPositions().Where(p => p.Y == exit.Position.Y
            && Math.Abs(exit.Position.X - p.X) > 1).First();
        var stepsUp = new Steps(position, leadDown: false,
            faceRight: exit.Direction.GetOpposite() == Direction.Right);
        room.AddEntity(stepsUp);
    }

    /// <summary>
    /// Creates <see cref="Steps"/> leading to the next level.
    /// </summary>
    /// <param name="room">Room where steps will be placed.</param>
    /// <exception cref="InvalidOperationException"></exception>
    static void CreateStepsDown(Room room)
    {
        var exit = room.Exits.FirstOrDefault() ??
            throw new InvalidOperationException("No valid exits in the room.");
        var position = room.Area.Center;
        var direction = exit.Direction.IsHorizontal() ?
            exit.Direction.GetOpposite() : Direction.Left;
        var stepsDown = new Steps(position, leadDown: true,
            faceRight: direction == Direction.Right);
        room.AddEntity(stepsDown);
    }
}