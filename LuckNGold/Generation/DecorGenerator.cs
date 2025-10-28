using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Map;
using LuckNGold.Primitives;
using LuckNGold.World.Common.Enums;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that creates decorators to fill empty space in dungeon rooms.
/// </summary>
internal class DecorGenerator() : GenerationStep("Decorators",
    new ComponentTypeTagPair(typeof(ItemList<Room>), "Rooms"))
{
    static readonly IEnhancedRandom s_rnd = GlobalRandom.DefaultRNG;
    static Size s_webSizeTracker = Size.Small;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var rooms = context.GetFirst<ItemList<Room>>("Rooms").Items;

        foreach (var room in rooms)
        {
            AddDecorToTopWall(room);
            AddDecorToCorners(room);
            AddDecorToSideWalls(room);
        }

        yield break;
    }

    static void AddDecorToCorners(Room room)
    {
        // Iterate through all corners of the room.
        for (int i = 0; i < 4; i++)
        {
            var cornerPosition = room.CornerPositions[i];
            HorizontalOrientation orientation = i == 0 || i == 2 ?
                HorizontalOrientation.Left : HorizontalOrientation.Right;
            Size size = s_rnd.NextBool() ? Size.Small : Size.Large;

            if (room.GetEntityAt(cornerPosition) is null)
            {
                // Add decor to corner of the room.
                switch (s_rnd.NextInt(4))
                {
                    // Large boxes.
                    case 0:
                        AddEntityOrWeb(room, orientation,
                            new Boxes(cornerPosition, Size.Large));
                        break;

                    // Candle or candle stand.
                    case 1:
                        bool addCandle = true;
                        if (cornerPosition.Y == room.Area.MinExtentY)
                        {
                            var testPosition = cornerPosition + Direction.Up;
                            var entity = room.GetEntityAt(testPosition);
                            addCandle = entity is Torch;
                        }

                        if (addCandle)
                        {
                            bool standAlone = s_rnd.NextBool();
                            if (standAlone)
                                AddEntityOrWeb(room, orientation,
                                    new CandleStand(cornerPosition, size));
                            else
                                AddEntityOrWeb(room, orientation,
                                    new Candle(cornerPosition, size));
                        }
                        else
                        {
                            AddWeb(room, orientation, cornerPosition);
                        }
                        break;

                    // Skull.
                    case 2:
                        AddEntityOrWeb(room, orientation,
                            new Skull(cornerPosition, orientation));
                        break;

                    // Cauldron:
                    case 3:
                        if (s_rnd.PercentageCheck(20f))
                        {
                            AddEntityOrWeb(room, orientation,
                                new Cauldron(cornerPosition), 1);
                        }
                        else
                            AddWeb(room, orientation, cornerPosition);
                        break;

                    // Spider web.
                    default:
                        AddWeb(room, orientation, cornerPosition);
                        break;
                };
            }
            
            // Check size is sufficient and add decor to two neighbour positions
            // of the corners of the room.
            if (room.Width >= 5)
            {
                var entity = room.GetEntityAt(cornerPosition);
                var cornerNeighbours = room.GetCornerNeighbours(cornerPosition);
                for (int j = 0; j < 2; j++)
                {
                    var neighbourPosition = cornerNeighbours[j];

                    if (room.GetEntityAt(neighbourPosition) is null)
                    {
                        // Leave larger number in nextint to reduce chance
                        // of generating decor.
                        switch (s_rnd.NextInt(8))
                        {
                            // Skull.
                            case 0:
                                if (entity is Skull)
                                    CheckCountAndAdd(room, new Bones(neighbourPosition));
                                else
                                    CheckCountAndAdd(room,
                                        new Skull(neighbourPosition, orientation));
                                break;

                            // Bones.
                            case 1:
                                CheckCountAndAdd(room, new Bones(neighbourPosition));
                                break;

                            // Amber stand (don't ask me what this is).
                            case 2:
                                CheckCountAndAdd(room, new AmberStand(neighbourPosition));
                                break;

                            // Small boxes.
                            case 3:
                                if (entity is Boxes boxes && boxes.Size == Size.Small)
                                    CheckCountAndAdd(room, new Bones(neighbourPosition));
                                else
                                    CheckCountAndAdd(room, 
                                        new Boxes(neighbourPosition, Size.Small));
                                break;

                            // Nothing.
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    static void AddWeb(Room room, HorizontalOrientation orientation, Point position)
    {
        s_webSizeTracker = s_webSizeTracker == Size.Small ? Size.Medium :
            s_webSizeTracker == Size.Medium ? Size.Large : Size.Small;
        room.AddEntity(new SpiderWeb(position, s_webSizeTracker, orientation));
    }

    static void AddEntityOrWeb(Room room, HorizontalOrientation orientation, 
        Entity entity, int maxCount = 2)
    {
        if (EntityCountIsLessThan(room, entity.GetType(), maxCount))
            room.AddEntity(entity);
        else
            AddWeb(room, orientation, entity.Position);
    }

    static void CheckCountAndAdd(Room room, Entity entity, int maxCount = 2)
    {
        if (EntityCountIsLessThan(room, entity.GetType(), maxCount))
            room.AddEntity(entity);
    }

    static bool EntityCountIsLessThan(Room room, Type type, int count)
    {
        int entityCount = room.Contents.Where(e => e.GetType() == type).Count();
        return entityCount < count;
    }

    static void AddDecorToSideWalls(Room room)
    {
        // Place side wall torches.
        foreach (var exit in room.Exits)
        {
            if (exit.Direction.IsHorizontal() && s_rnd.PercentageCheck(35f))
                AddSideTorches(exit);
        }
    }

    static void AddDecorToTopWall(Room room)
    {
        var topWallCenter = room.GetConnectionPoint(Direction.Up);

        // Place decor in the middle of the top wall.
        if (!room.TryGetExit(Direction.Up, out Exit? _)
            && room.PositionIsFree(topWallCenter)
            && s_rnd.NextBool())
        {
            if (room.Width.IsOdd())
            {
                if (s_rnd.NextBool() && room.PositionIsFree(topWallCenter + Direction.Down))
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
        if (((double)topWallEntities.Length + 2) / room.Width < 0.7d)
        {
            // Find two free top wall positions.
            List<Point> freeTopWallPositions = [];
            int x = 0;
            do x = s_rnd.NextInt(room.Position.X, topWallCenter.X);
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
                placeShackles = s_rnd.NextBool();
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
    }

    static void AddShackle(Point position, Room room)
    {
        var shackleSize = s_rnd.NextBool() ? "Small" : "Large";
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
        bool isBlue = s_rnd.NextBool();

        // Check neighbour rooms if they have any fountains
        // and if so, change color to opposite.
        foreach (var exit in room.Exits)
        {
            var neighbourRoom = exit.End!.Room;
            var neighbourRoomFountain = room.GetEntity<Fountain>();
            if (neighbourRoomFountain is not null)
            {
                isBlue = !neighbourRoomFountain.IsBlue;
                break;
            }
        }

        var fountainTop = new Fountain(wallCenter, VerticalOrientation.Top, isBlue);
        var fountainBottom = new Fountain(wallCenter + Direction.Down,
            VerticalOrientation.Bottom, isBlue);
        room.AddEntity(fountainTop);
        room.AddEntity(fountainBottom);
    }

    static void AddSideTorches(Exit exit)
    {
        var room = exit.Room;
        var horizontalDelta = exit.Direction.GetOpposite();
        var verticalDelta = s_rnd.NextInt(1, (exit.Room.Height - 1) / 2);

        Point[] positions = [exit.Position + horizontalDelta - (0, verticalDelta),
            exit.Position + horizontalDelta + (0, verticalDelta)];
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
            room.AddEntity(torch);
    }
}