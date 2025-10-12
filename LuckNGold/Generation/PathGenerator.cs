using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.Generation;

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
        var rooms = GetRooms(context);
        if (rooms.Count == 0) 
            rooms = [room];

        var mapBounds = new Rectangle(0, 0, context.Width, context.Height);

        // handles random number gen
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
            int roomsInLine = GetRoomsInLine(oppositeDirection, room);

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
                    CreatePassage(room, newRoom, direction, corridors);

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

    static void CreatePassage(Room firstRoom, Room secondRoom, Direction direction,
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

    protected void AddRoomPathsToContext(GenerationContext context, RoomPath roomPath)
    {
        if (roomPath.Count == 0)
            throw new ArgumentException("Paths with zero lengths are not allowed.");

        // add room path to context
        var pathContext = GetPaths(context);
        pathContext.Add(roomPath, Name);

        // set all room area positions in the wallFloorContext to true (walkable)
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

        // set all corridor positions in the wallFloorContext to true (walkable)
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

    protected static List<Room> GetRooms(GenerationContext context)
    {
        var paths = GetPaths(context);
        var rooms = new List<Room>();
        foreach (var element in paths)
            rooms.AddRange(element.Item.Rooms);
        return rooms;
    }

    // returns rooms connected in the same line vertically or horizontally
    static int GetRoomsInLine(Direction direction, Room room)
    {
        int count = 1;
        while (true)
        {
            if (!room.TryGetExit(direction, out Exit? exit))
                break;
            Exit? end = exit.End;
            if (end is null)
                break;
            room = end.Room;
            count++;
        }
        return count;
    }
}