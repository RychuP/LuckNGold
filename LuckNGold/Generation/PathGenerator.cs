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
    const int MaxRoomsWithDoorInLine = 3;

    const int MinCorridorLength = 2;
    const int MaxCorridorLength = 4;

    /// <summary>
    /// Generates rooms for the <see cref="RoomPath"/>.
    /// </summary>
    /// <param name="roomPath"><see cref="RoomPath"/> which will store the rooms.</param>
    /// <param name="corridors"><see cref="Corridor"/>s created between the rooms in the path.</param>
    /// <param name="rooms">List of all rooms from the context used as a local copy.</param>
    /// <param name="startRoom">First <see cref="Room"/> where the path starts.</param>
    /// <param name="roomCount">Number of rooms to create for the path.</param>
    /// <param name="mapBounds">Bounds of the map where all the rooms need to fit.</param>
    protected static void CreateRooms(ref RoomPath roomPath, ref List<Corridor> corridors,
        List<Room> rooms, Room startRoom, int roomCount, Rectangle mapBounds)
    {
        // handles random number gen
        var rnd = GlobalRandom.DefaultRNG;

        // current node
        var room = startRoom;

        // direction for probing available space to place a room
        Direction direction = Direction.None;

        // keeps track of creating rooms in the same direction
        Direction lastCorridorDirection = direction;

        // it starts off as one but as the gen goes on it defaults to two
        // every change of direction results in the last two rooms being in the same line
        int roomsInLine = 1;

        // create a chain of rooms
        while (roomPath.Count < roomCount)
        {
            // check if the room has walls that can accept new exits
            if (!room.HasAvailableConnections())
                break;

            // establish delimiters for maximum number of rooms in one line
            // prefer not to line up many rooms with wide corridors
            // lining up vertically in one chain
            bool roomsWithDoorsLinedUpVertically = room.Width.IsEven() &&
                lastCorridorDirection.IsVertical();
            var maxRoomsInLine = roomsWithDoorsLinedUpVertically ?
                MaxRoomsWithDoorInLine : MaxRoomsInLine;

            // get random available direction for an exit from the room
            Direction tempDirection;
            do
            {
                tempDirection = room.GetRandomAvailableConnection();

                // if there is only one remaining connection available
                // break out as there is no point trying over and over
                if (room.Connections.Count == 3)
                    break;
            }
            while (lastCorridorDirection == tempDirection &&
                roomsInLine >= maxRoomsInLine);

            direction = tempDirection;

            // create a probe with the given room and direction
            int corridorLength = rnd.NextInt(MinCorridorLength, MaxCorridorLength + 1);
            var probe = new Probe(room, direction, corridorLength);
            try
            {
                // check if there is enough space to place a new room
                if (probe.CheckArea(rooms, mapBounds))
                {
                    int height, width = 0, attemptCount = 0;
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
                        || sizeRatio < 0.65d) && attemptCount < 100);

                    // get position for the new room
                    var pos = room.GetAdjacentAreaPosition(direction,
                        probe.PathLength, width, height);

                    // create the new room
                    Room newRoom = new(pos, width, height);

                    // add room to the list of rooms in the path
                    roomPath.Add(newRoom);
                    rooms.Add(newRoom);

                    // create exits and corridor
                    CreatePassage(room, newRoom, direction, corridors);

                    // check if the latest path direction is the same as the prev one
                    if (direction == lastCorridorDirection)
                        roomsInLine++;

                    // reset the counter if the direction has changed
                    // after the direction change there are now 2 rooms in the new line
                    else
                        roomsInLine = 2;

                    // remember the last corridor direction
                    lastCorridorDirection = direction;

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
    }

    protected void AddRoomsToContext(GenerationContext context, RoomPath roomPath)
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
}