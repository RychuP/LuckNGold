using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Map;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.Generation;

/// <summary>
/// Base class for all other path generators providing common functionality.
/// </summary>
/// <param name="name">Name of the path.</param>
/// <param name="required">Components required for the proper functionality 
/// of this generator.</param>
abstract class PathGenerator(string? name = null, params ComponentTypeTagPair[] required) 
    : GenerationStep(name, required)
{
    // max number of rooms that can appear in one line in path
    const int MaxRoomsInLine = 4;

    // as above but for the rooms with double path where doors can be placed
    const int MaxRoomsWithWideExitInLine = 2;

    // min max corrid length between rooms
    const int MinCorridorLength = 2;
    const int MaxCorridorLength = 4;

    /// <summary>
    /// Generates rooms for the <see cref="RoomPath"/>.
    /// </summary>
    /// <param name="roomPath"><see cref="RoomPath"/> which will store the rooms.</param>
    /// <param name="corridors"><see cref="Corridor"/>s created between the rooms in the path.</param>
    /// <param name="room">First <see cref="Room"/> where the path starts.</param>
    /// <param name="roomCount">Number of rooms to create for the path.</param>
    protected static void CreateRooms(ref RoomPath roomPath, ref List<Corridor> corridors,
        GenerationContext context, Room room, int roomCount)
    {
        // Get a copy of all rooms so that the new ones can be added in and they will all
        // be taken into account when checking empty space before placing a new room.
        var rooms = GetClonedRooms(context);
        if (rooms.Count == 0)
            rooms = [room];

        var mapBounds = new Rectangle(0, 0, context.Width, context.Height);
        var rnd = GlobalRandom.DefaultRNG;

        // create a chain of rooms
        while (roomPath.Count < roomCount)
        {
            // check if the room has walls that can accept new exits
            if (!room.HasAvailableConnections())
                break;

            // desired direction in which to continue adding rooms
            Direction direction = room.GetRandomAvailableConnection();

            // establish how many rooms max in this type of line
            var maxRoomsInLine = direction.IsVertical() && room.Width.IsEven() ?
                MaxRoomsWithWideExitInLine : MaxRoomsInLine;

            // opposite direction to check how many rooms there are already in line
            var oppositeDirection = direction.GetOpposite();
            int roomsInLine = CountRoomsInLine(oppositeDirection, room);

            // max rooms in line reached -> mark this connection as dead end
            if (roomsInLine >= maxRoomsInLine)
            {
                room.AddDeadEnd(direction);

                // come back to checking if there any more available connections
                continue;
            }

            // create a probe with the given room and direction
            int corridorLength = rnd.NextInt(MinCorridorLength, MaxCorridorLength + 1);
            var probe = new Probe(room, direction, corridorLength);
            try
            {
                // check if there is enough space to place a new room
                if (probe.CheckArea(rooms, mapBounds))
                {
                    int height, width = 0, attemptCount = 0, maxAttempts = 100;
                    double sizeRatio;
                    do
                    {
                        attemptCount++;

                        // calculate size of the new room
                        height = Room.GetRandomOddSize(Room.MinOddSize, probe.Height);
                        if (direction.IsHorizontal())
                            width = Room.GetRandomSize(max: probe.Width);
                        else if (direction.IsVertical())
                            width = room.GetWallSize(direction).IsEven() ?
                                Room.GetRandomEvenSize(Room.MinEvenSize, probe.Width) :
                                Room.GetRandomOddSize(max: probe.Width);

                        // calculate width/height ratio
                        double i = Math.Min(height, width);
                        double j = Math.Max(height, width);
                        sizeRatio = i / j;
                    }
                    // try to make the rooms less elongated
                    while ((room.Width == width || room.Height == height 
                        || sizeRatio < 0.65d) && attemptCount <= maxAttempts);

                    if (attemptCount > maxAttempts)
                        throw new ProbeException("Possibly an awkward shape room created.");

                    // get position for the new room
                    var pos = room.GetAdjacentAreaPosition(direction,
                        probe.PathLength, width, height);

                    // create the new room
                    Room newRoom = new(pos, width, height, roomPath);

                    // add room to the list of rooms in the path
                    roomPath.Add(newRoom);
                    rooms.Add(newRoom);

                    // create exits and corridor
                    CreateCorridor(room, newRoom, direction, corridors);

                    // save the new room as the current node
                    room = newRoom;
                }

            }
            catch (ProbeException)
            {
                room.AddDeadEnd(direction);
            }
        }
    }

    /// <summary>
    /// Creates a corridor and associated exits between rooms.
    /// </summary>
    /// <param name="firstRoom"><see cref="Room"/> where <see cref="Corridor"/> starts.</param>
    /// <param name="secondRoom"><see cref="Room"/> where <see cref="Corridor"/> ends.</param>
    /// <param name="direction"><see cref="Direction"/> of <see cref="Corridor"/>.</param>
    /// <param name="corridors">List of all corridors to which the new one will be added.</param>
    static void CreateCorridor(Room firstRoom, Room secondRoom, Direction direction,
        List<Corridor> corridors)
    {
        // create exits
        var start = firstRoom.AddExit(direction);
        var end = secondRoom.AddExit(direction.GetOpposite());
        
        // create corridor
        var corridor = new Corridor(start, end);
        corridors.Add(corridor);

        // add corridor to exits
        start.Corridor = corridor;
        end.Corridor = corridor;
    }

    protected void AddPathsToContext(GenerationContext context, RoomPath roomPath)
    {
        if (roomPath.Count == 0)
            throw new ArgumentException("Paths with zero lengths are not allowed.");

        // Add room path to context.
        GetPaths(context).Add(roomPath, Name);
    }

    protected void AddRoomsToContext(GenerationContext context, RoomPath roomPath)
    {
        if (roomPath.Count == 0)
            return;

        // Add rooms to context.
        GetRooms(context).AddRange(roomPath.Rooms, Name);

        // Set all room area positions in the wallFloorContext to true (walkable).
        var wallFloorContext = GetWallFloor(context);
        foreach (var room in roomPath.Rooms)
        {
            foreach (var point in room.Area.Positions())
                wallFloorContext[point] = true;
        }
    }

    protected void AddCorridorsToContext(GenerationContext context, List<Corridor> corridors)
    {
        var wallFloorContext = GetWallFloor(context);
        GetCorridors(context).AddRange(corridors, Name);

        // Set all corridor positions in the wallFloorContext to true (walkable).
        foreach (var corridor in corridors)
        {
            foreach (var point in corridor.GetPositions())
                wallFloorContext[point] = true;
        }
    }

    static ISettableGridView<bool> GetWallFloor(GenerationContext context) =>
        context.GetFirstOrNew<ISettableGridView<bool>>(
            () => new ArrayView<bool>(context.Width, context.Height),
            "WallFloor"
        );

    protected static ItemList<RoomPath> GetPaths(GenerationContext context) =>
        context.GetFirstOrNew(() => new ItemList<RoomPath>(), "Paths");

    static ItemList<Corridor> GetCorridors(GenerationContext context) =>
        context.GetFirstOrNew(() => new ItemList<Corridor>(), "Corridors");

    static ItemList<Room> GetRooms(GenerationContext context) =>
        context.GetFirstOrNew(() => new ItemList<Room>(), "Rooms");

    protected static List<Room> GetClonedRooms(GenerationContext context) =>
        [.. GetRooms(context).Items];

    /// <summary>
    /// Counts rooms connected in the same line vertically or horizontally.
    /// </summary>
    /// <param name="direction">Direction in which to start checking exits 
    /// and counting rooms.</param>
    /// <param name="room">Start room where the counting starts.</param>
    /// <returns></returns>
    static int CountRoomsInLine(Direction direction, Room room)
    {
        int count = 1;
        while (true)
        {
            if (!room.TryGetExit(direction, out Exit? exit))
                break;
            room = exit.End!.Room;
            count++;
        }
        return count;
    }
}