using GoRogue.MapGeneration;
using GoRogue.Random;
using SadConsole.EasingFunctions;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

internal class MainPathGenerator(int roomCount) : PathGenerator("MainPath")
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        // map bounds for the purpose of placing rooms
        Rectangle mapBounds = new(0, 0, context.Width, context.Height);

        // list of rooms in the chain
        RoomPath mainPath = new(Name);

        // list of corridors between the rooms in the chain
        List<Corridor> corridors = [];

        while (mainPath.Count < roomCount)
        {
            mainPath.Clear();
            corridors.Clear();

            var area = GetFirstRoom(mapBounds);
            var firstRoom = new Room(area.Position, area.Width, area.Height, mainPath);
            firstRoom.AddDeadEnd(Direction.Up);
            firstRoom.AddDeadEnd(Direction.Down);
            mainPath.Add(firstRoom);

            CreateRooms(ref mainPath, ref corridors, context, firstRoom, roomCount);
        }

        // add dead ends to first and last room, so that they won't
        // accept any further connections
        AddDeadEnds(mainPath.FirstRoom);
        AddDeadEnds(mainPath.LastRoom);

        // update context
        AddRoomPathsToContext(context, mainPath);
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

    static Rectangle GetFirstRoom(Rectangle bounds)
    {
        var rnd = GlobalRandom.DefaultRNG;
        int width = 0, height = 0;
        double sizeRatio = 0;

        // try to make the rooms less elongated
        while (sizeRatio < Room.MinSizeRatio)
        {
            width = Room.GetRandomOddSize(max: 5);
            height = Room.GetRandomOddSize(max: 5);

            // calculate width/height ratio
            double i = Math.Min(height, width);
            double j = Math.Max(height, width);
            sizeRatio = i / j;
        }

        // extra 2 cells for walls on each side
        Rectangle searchArea = new(1, 1, bounds.Width - width - 2, bounds.Height - height - 2);

        // random position that will allow the room to fully fit within the map bounds
        var pos = rnd.RandomPosition(searchArea);
        
        return new Rectangle(pos.X, pos.Y, width, height);
    }
}