using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Map;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that creates decorators to fill empty space in dungeon rooms.
/// </summary>
internal class DecorGenerator() : GenerationStep("Decorators",
    new ComponentTypeTagPair(typeof(ItemList<Room>), "Rooms"))
{
    static readonly IEnhancedRandom _rnd = GlobalRandom.DefaultRNG;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var rooms = context.GetFirst<ItemList<Room>>("Rooms").Items;

        foreach (var room in rooms)
        {
            var topWallCenter = room.GetConnectionPoint(Direction.Up);

            // Place an entity in the middle of the top wall.
            if (!room.TryGetExit(Direction.Up, out Exit? _) 
                && room.PositionIsFree(topWallCenter)
                && _rnd.NextBool())
            {
                if (room.Width.IsOdd())
                {
                    if (_rnd.NextBool())
                        AddFountain(room);
                    else
                        AddTorch(topWallCenter, room);
                }
                else
                {
                    AddShackle(topWallCenter, room);
                    AddShackle(topWallCenter + Direction.Right, room);
                }
            }

            // Fill the remaining top wall space.
            var topWallEntities = room.GetEntitiesAtY(room.Bounds.MinExtentY);
            if ((double)topWallEntities.Length / room.Width < 0.7d)
            {
                // Find two free top wall positions.
                List<Point> freeTopWallPositions = [];
                int x = 0;
                do x = _rnd.NextInt(room.Position.X, topWallCenter.X);
                while (Array.Find(topWallEntities, e => e.Position.X == x) is not null);
                freeTopWallPositions.Add(new Point(x, room.Bounds.MinExtentY));
                int delta = topWallCenter.X - x;
                x = room.Width.IsOdd() ? topWallCenter.X + delta :
                    topWallCenter.X + delta + 1;
                freeTopWallPositions.Add(new Point(x, room.Bounds.MinExtentY));

                // Place either shackles or torches depending what's already there.
                bool placeShackles = true;
                if (!room.Contains<Torch>() && !room.Contains<Shackle>())
                {
                    placeShackles = _rnd.NextBool();
                }
                else if (room.Contains<Shackle>())
                {
                    placeShackles = false;
                }

                foreach (var point in freeTopWallPositions)
                {
                    if (placeShackles)
                        AddShackle(point, room);
                    else
                        AddTorch(point, room);
                }
            }

            // Place side torches next to horizontal exits.
            foreach (var exit in room.Exits)
            {
                if (exit.Direction.IsHorizontal() && _rnd.PercentageCheck(35f))
                    AddSideTorches(exit);
            }
        }

        yield break;
    }

    static void AddShackle(Point position, Room room)
    {
        var shackleSize = _rnd.NextBool() ? "Small" : "Large";
        var shackle = new Shackle(position, shackleSize);
        room.AddEntity(shackle);
    }

    static void AddTorch(Point position, Room room)
    {
        var torch = new Torch(position);
        room.AddEntity(torch);
    }

    static void AddFountain(Room room)
    {
        var wallCenter = room.GetConnectionPoint(Direction.Up);
        if (room.PositionIsFree(wallCenter)
            && room.PositionIsFree(wallCenter + Direction.Down))
        {
            var fountainTop = new FountainTop(wallCenter);
            var fountainBottom = new FountainBottom(wallCenter + Direction.Down);
            room.AddEntity(fountainTop);
            room.AddEntity(fountainBottom);
        }
    }

    static void AddSideTorches(Exit exit)
    {
        var room = exit.Room;
        var delta = exit.Direction.GetOpposite();

        Point[] positions = [exit.Position + delta + Direction.Up,
            exit.Position + delta + Direction.Down];
        var torches = new SideTorch[2];

        for (int i = 0; i < positions.Length; i++) 
        {
            var position = positions[i];

            // Skip adding torches if another light source is already placed nearby.
            var lightSourcePositions = AdjacencyRule.Cardinals.Neighbors(position);
            foreach (var point in lightSourcePositions)
            {
                var entity = room.GetEntityAt(point);
                if (entity is Torch)
                    return;
            }

            // Skip adding torches if an entity is present where a torch should be.
            if (!room.PositionIsFree(position))
                return;
            else
                torches[i] = new SideTorch(position, exit.Direction);
        }

        foreach (var torch in torches)
        {
            

            room.AddEntity(torch);
        }
    }
}