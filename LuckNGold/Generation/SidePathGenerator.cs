using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;

namespace LuckNGold.Generation;

internal class SidePathGenerator() : PathGenerator("SidePath", 
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

        // keep adding side paths until all space is blocked around main path
        List<Room> roomsWithFreeConnections;
        for (int i = 0; i < 100; i++)
        {
            // update the list of rooms with free connections
            roomsWithFreeConnections = GetRoomsWithFreeConnections(mainPath);
            if (roomsWithFreeConnections.Count == 0)
                break;

            // get a random room with free connections
            int index = rnd.NextInt(roomsWithFreeConnections.Count);
            var startRoom = roomsWithFreeConnections[index];

            // create a new path
            var sidePath = new RoomPath(Name, mainPath, startRoom);

            // random number of rooms in the path
            var roomCount = rnd.NextInt(MinPathLength, MaxPathLength + 1);

            CreateRooms(ref sidePath, ref corridors, context, startRoom, roomCount);

            if (sidePath.Count == 0)
                continue;

            mainPath.SideRoomCount += sidePath.Count;

            // update context
            AddRoomPathsToContext(context, sidePath);
        }

        AddCorridorsToContext(context, corridors);

        yield break;
    }

    static List<Room> GetRoomsWithFreeConnections(RoomPath path) =>
        [.. path.Rooms.Where(r => r.HasAvailableConnections()).Select(r => r)];
}