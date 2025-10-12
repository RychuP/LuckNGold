using GoRogue.MapGeneration;
using GoRogue.Random;
using LuckNGold.Visuals.Screens;
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

        double distance;
        var minDistance = context.Width * 0.5d;

        do
        {
            mainPath.Clear();
            corridors.Clear();

            var area = GetFirstRoom(mapBounds);
            var firstRoom = new Room(area.Position, area.Width, area.Height, mainPath);
            firstRoom.AddDeadEnd(Direction.Up);
            firstRoom.AddDeadEnd(Direction.Down);
            mainPath.Add(firstRoom);

            CreateRooms(ref mainPath, ref corridors, context, firstRoom, roomCount);

            distance = Program.Distance.Calculate(mainPath.FirstRoom.Area.Center,
                mainPath.LastRoom.Area.Center);
        }
        // keep generating the main path until the required number of rooms is achieved
        // and the percentage of dead ends to available connections doesn't go over 
        // a certain small treshold
        while (mainPath.Count < roomCount || distance < minDistance);

        GameScreen.Print($"Min distance: {minDistance:0.00}");
        GameScreen.Print($"Actual distance: {distance:0.00}");

        // add dead ends to first and last room, so that they won't
        // accept any further connections
        AddDeadEnds(mainPath.FirstRoom);
        AddDeadEnds(mainPath.LastRoom);

        // update context
        AddRoomPathsToContext(context, mainPath);
        AddCorridorsToContext(context, corridors);

        yield break;
    }

    // Checks the middle part of the main path to see if the rooms have plenty 
    // of available connections to accept side paths in the next generation step
    static bool CheckDeadEnds(RoomPath path, double percent)
    {
        int deadEndCount = 0;
        for (int i = 1;  i < path.Count - 1; i++)
        {
            var room = path.Rooms[i];
            foreach (var connection in room.Connections)
            {
                if (connection is DeadEnd)
                    deadEndCount++;
            }
        }
        var possibleConnections = (path.Count - 2) * 4;
        return possibleConnections * percent > deadEndCount;
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
        //int width = 0, height = 0;
        //double sizeRatio = 0;
        int width = 5, height = 5;

        // try to make the rooms less elongated
        //while (sizeRatio < Room.MinSizeRatio)
        //{
        //    width = Room.GetRandomOddSize(max: 5);
        //    height = Room.GetRandomOddSize(max: 5);

        //    // calculate width/height ratio
        //    double i = Math.Min(height, width);
        //    double j = Math.Max(height, width);
        //    sizeRatio = i / j;
        //}

        // extra 2 cells for walls on each side
        Rectangle searchArea = new(1, 1, bounds.Width - width - 2, bounds.Height - height - 2);

        // random position that will allow the room to fully fit within the map bounds
        var pos = rnd.RandomPosition(searchArea);
        
        return new Rectangle(pos.X, pos.Y, width, height);
    }
}