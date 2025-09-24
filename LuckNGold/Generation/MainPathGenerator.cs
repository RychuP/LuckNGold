using GoRogue.MapGeneration;
using GoRogue.Random;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

internal class MainPathGenerator(int roomCount) : PathGenerator("MainPath")
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        // map bounds for the purpose of placing rooms
        Rectangle mapBounds = new(0, 0, context.Width, context.Height);

        // list of rooms in the chain
        RoomPath roomPath = new(Name);

        // list of corridors between the rooms in the chain
        List<Corridor> corridors = [];

        while (roomPath.Count < roomCount)
        {
            roomPath.Clear();
            corridors.Clear();

            var firstRoom = GetFirstRoom(mapBounds);
            var rooms = GetRooms(context);
            roomPath.Add(firstRoom);
            rooms.Add(firstRoom);

            CreateRooms(ref roomPath, ref corridors, rooms, firstRoom, roomCount, mapBounds);
        }

        // add dead ends to first and last room, so that they won't
        // accepty any further connections
        AddDeadEnds(roomPath.FirstRoom);
        AddDeadEnds(roomPath.LastRoom);

        // update context
        AddRoomsToContext(context, roomPath);
        AddCorridorsToContext(context, corridors);

        yield break;
    }

    static void AddDeadEnds(Room room)
    {
        foreach (var direction in AdjacencyRule.Cardinals.DirectionsOfNeighbors())
        {
            if (room.Connections.Any(c => c.Direction == direction))
                continue;
            else
                room.AddDeadEnd(direction);
        }
    }

    static Room GetFirstRoom(Rectangle bounds)
    {
        var rnd = GlobalRandom.DefaultRNG;
        int width = Room.GetRandomSize();
        int height = Room.GetRandomOddSize();
        Rectangle searchArea = new(0, 0, bounds.Width - width, bounds.Height - height);
        var pos = rnd.RandomPosition(searchArea);
        return new Room(pos, width, height);
    }
}