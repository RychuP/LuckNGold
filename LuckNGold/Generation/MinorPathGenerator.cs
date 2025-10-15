using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;

namespace LuckNGold.Generation;

internal class MinorPathGenerator() : PathGenerator("MinorPath",
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"))
{
    // min length of the initial side path
    const int MinPathLength = 1;

    // max length of the initial side path
    // it can change later when more legs are added
    const int MaxPathLength = 5;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var rnd = GlobalRandom.DefaultRNG;

        // check present paths
        var pathContext = GetPaths(context);
        if (pathContext.Items.Count == 0)
            throw new InvalidOperationException("At least one path is required.");
        var mainPath = pathContext.Items[0];
        if (pathContext.ItemToStepMapping[mainPath] != "MainPath")
            throw new ArgumentException("Main path is required.");

        // list of corridors created in this step
        List<Corridor> corridors = [];

        // list of rooms with free connections (can accept new exits)
        List<Room> roomsWithFreeConnections = [];

        // keep trying adding more rooms until all space is filled
        int attemptCount = 0;
        do
        {
            roomsWithFreeConnections.Clear();

            // update the list of rooms with free connections from all paths
            foreach (var bucket in pathContext)
            {
                RoomPath path = bucket.Item;
                var pathRoomsWithFreeConnections = GetRoomsWithFreeConnections(path);
                roomsWithFreeConnections.AddRange(pathRoomsWithFreeConnections);
            }

            // mission complete
            if (roomsWithFreeConnections.Count == 0)
                break;

            // get a random room with free connections
            int index = rnd.NextInt(roomsWithFreeConnections.Count);
            var startRoom = roomsWithFreeConnections[index];

            // create a new path
            var minorPath = new RoomPath(Name, startRoom.Path, startRoom);

            // random number of rooms in the path
            var roomCount = rnd.NextInt(MinPathLength, MaxPathLength + 1);

            CreateRooms(ref minorPath, ref corridors, context, startRoom, roomCount);

            if (minorPath.Count == 0)
            {
                // nothing to add to context
                continue;
            }

            // Transfer rooms from minor paths that form an extension of an existing path
            // to the parent path.
            if (startRoom.Path.LastRoom == startRoom)
            {
                AddRoomsToContext(context, minorPath);

                // Update parent of the parent with the number of side rooms from the minor path
                // that will be added to the parent path
                minorPath.Parent!.Parent!.SideRoomCount += minorPath.Count;

                // Transfer rooms to the parent path.
                while (minorPath.Count > 0)
                {
                    var room = minorPath.FirstRoom;
                    if (!room.TransferToPath(startRoom.Path))
                        throw new InvalidOperationException("Room transfer between paths failed.");
                }

                // Skip adding minor path to the context as it has been emptied.
                continue;
            }

            // Update parent with the number of side rooms from the minor path
            minorPath.Parent!.SideRoomCount += minorPath.Count;

            // update context
            AddPathsToContext(context, minorPath);
            AddRoomsToContext(context, minorPath);
        }
        while (roomsWithFreeConnections.Count > 0 && attemptCount++ < 200);

        AddCorridorsToContext(context, corridors);

        yield break;
    }

    static List<Room> GetRoomsWithFreeConnections(RoomPath path) =>
        [.. path.Rooms.Where(r => r.HasAvailableConnections())];
}