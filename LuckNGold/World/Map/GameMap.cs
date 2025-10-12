using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation;
using LuckNGold.Visuals;
using LuckNGold.World.Decor;
using LuckNGold.World.Furniture;
using LuckNGold.World.Furniture.Components;
using LuckNGold.World.Furniture.Enums;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map.Components;
using SadRogue.Integration.Maps;

namespace LuckNGold.World.Map;

/// <summary>
/// Map handles everything that happens inside the game world.
/// </summary>
class GameMap : RogueLikeMap
{
    // Map width/height
    public const int DefaultWidth = 100;
    public const int DefaultHeight = 60;

    // Effectively max view zoom level
    const int MaxFontSizeMultiplier = 4;

    // Effectively current view zoom level
    int _fontSizeMultiplier = 3;

    // List of all rooms from generator for testing/debugging
    public IReadOnlyList<Room> Rooms { get; init; }

    // List of all paths from generator for testing/debugging
    // (path connects many rooms in a logical, linear way)
    public IReadOnlyList<RoomPath> Paths { get; init; }

    // list of all corridors from generator for testing/debugging
    // (corridor connects two rooms)
    public IReadOnlyList<Corridor> Corridors { get; init; }

    /// <summary>
    /// Layers available for entities in the game
    /// </summary>
    public enum Layer
    {
        // The basic structure of the level (walls, floors, corridors). 
        Terrain = 0,

        // Non-interactive entities like rubble, flags, torches and other env details. 
        Decor,

        // Entities that can be interacted with like doors, chests, levers, etc
        Furniture,

        // Loot, tools, and other collectibles that can be picked up. Also projectiles ??
        Items,

        // Non-player characters, enemies, and the player itself. 
        Monsters
    }

    // CUSTOMIZATION: Change the distance from Distance.Chebyshev to whatever is desired for your game. By default,
    // this will affect the FOV shape as well as the distance calculation used for AStar pathfinding on the Map.
    public GameMap(GenerationContext context) : base(context.Width, context.Height, 
        null, Enum.GetValues<Layer>().Length - 1, Distance.Chebyshev)
    {
        // TODO Delete this in the future when generation is sorted.
        // Generated data saved mainly for testing and debugging purposes.
        Paths = context.GetFirst<ItemList<RoomPath>>().Items;
        Corridors = context.GetFirst<ItemList<Corridor>>().Items;
        var temp = new List<Room>();
        foreach (var path in Paths)
            temp.AddRange(path.Rooms);
        Rooms = temp;

        // Create renderer
        Point viewSize = new(Program.Width / _fontSizeMultiplier,
            Program.Height / _fontSizeMultiplier);
        DefaultRenderer = CreateRenderer(viewSize);
        DefaultRenderer.Font = Program.Font;
        DefaultRenderer.FontSize *= _fontSizeMultiplier;

        // Change default bg color to match wall color
        DefaultRenderer.Surface.DefaultBackground = Colors.Wall;
        DefaultRenderer.Surface.Clear();

        // fov handler
        AllComponents.Add(new MapFOVHandler());
    }

    public void ResizeView(int fontSizeMultiplier)
    {
        if (fontSizeMultiplier < 0 || fontSizeMultiplier > 4) return;
        var width = Program.Width / fontSizeMultiplier;
        var height = Program.Height / fontSizeMultiplier;
        DefaultRenderer!.Surface.View = new Rectangle(0, 0, width, height);
        var size = DefaultRenderer.Font.GetFontSize(IFont.Sizes.One) * fontSizeMultiplier;
        DefaultRenderer.FontSize = size;
    }

    public void ZoomViewIn()
    {
        if (_fontSizeMultiplier >= MaxFontSizeMultiplier) return;
        ResizeView(++_fontSizeMultiplier);
    }

    public void ZoomViewOut()
    {
        if (_fontSizeMultiplier <= 1) return;
        ResizeView(--_fontSizeMultiplier);
    }

    public void PlaceDoorAndKeys(ItemList<Door> doorList)
    {
        foreach (var itemStep in doorList)
        {
            Door genDoor = itemStep.Item;

            // Establish orientation of the door
            var direction = genDoor.Exit.Direction;
            DoorOrientation doorOrientation;
            if (direction.IsHorizontal())
            {
                doorOrientation = direction == Direction.Left ?
                    DoorOrientation.Left : DoorOrientation.Right;
            }
            else
            {
                doorOrientation = direction == Direction.Up ?
                    DoorOrientation.TopLeft : DoorOrientation.BottomLeft;
            }

            // Check if the door is locked.
            bool locked = false;
            Difficulty difficulty = Difficulty.None;
            if (genDoor.Lock is Lock @lock)
            {
                locked = true;
                difficulty = @lock.Difficulty;

                // Create the key
                var key = ItemFactory.Key((Gemstone) difficulty);
                key.Position = @lock.Key.Position;
                AddEntity(key);
            }

            // Create the door
            var door = FurnitureFactory.Door(doorOrientation, locked, difficulty);
            door.IsVisible = false;
            door.Position = genDoor.Exit.Position;
            AddEntity(door);

            // Add aditional door to wide corridors
            if (genDoor.Exit.IsDouble)
            {
                if (!direction.IsVertical())
                    throw new InvalidOperationException("Double door can only be vertical.");
                doorOrientation = direction == Direction.Up ?
                    DoorOrientation.TopRight : DoorOrientation.BottomRight;
                var door2 = FurnitureFactory.Door(doorOrientation, locked, difficulty);
                door2.IsVisible = false;
                door2.Position = door.Position + Direction.Right;
                AddEntity(door2);

                // Add mirror behaviours to both doors
                door.AllComponents.GetFirst<OpeningComponent>().Opened +=
                    (o, e) => door2.AllComponents.GetFirst<OpeningComponent>().Open();
                door.AllComponents.GetFirst<OpeningComponent>().Closed +=
                    (o, e) => door2.AllComponents.GetFirst<OpeningComponent>().Close();
                door2.AllComponents.GetFirst<OpeningComponent>().Opened +=
                    (o, e) => door.AllComponents.GetFirst<OpeningComponent>().Open();
                door2.AllComponents.GetFirst<OpeningComponent>().Closed +=
                    (o, e) => door.AllComponents.GetFirst<OpeningComponent>().Close();
                door.AllComponents.ComponentRemoved += (o, e) =>
                {
                    
                    if (e.Component is LockComponent lockComp)
                    {
                        var @lock = door2.AllComponents.GetFirstOrDefault<LockComponent>();
                        if (@lock != null)
                        {
                            var unlocker = new UnlockingComponent((Quality)lockComp.Difficulty);
                            @lock.Unlock(unlocker);
                        }
                    }
                };
                door2.AllComponents.ComponentRemoved += (o, e) =>
                {

                    if (e.Component is LockComponent lockComp)
                    {
                        var @lock = door.AllComponents.GetFirstOrDefault<LockComponent>();
                        if (@lock != null)
                        {
                            var unlocker = new UnlockingComponent((Quality)lockComp.Difficulty);
                            @lock.Unlock(unlocker);
                        }
                    }
                };
            }
        }
    }

    /// <summary>
    /// Places steps to the upper and lower level.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void PlaceSteps()
    {
        var mainPath = Paths[0];

        // Steps up
        var firstRoom = mainPath.FirstRoom;
        var exit = firstRoom.Connections.Find(c => c is Exit exit) as Exit ??
            throw new InvalidOperationException("No valid exits in the first room.");
        var position = firstRoom.Area.PerimeterPositions().Where(p => p.Y == exit.Position.Y
            && Math.Abs(exit.Position.X - p.X) > 1).First();
        var stepsUp = DecorFactory.Steps(exit.Direction.GetOpposite(), Direction.Up);
        stepsUp.Position = position;
        AddEntity(stepsUp);

        // Steps down
        var lastRoom = mainPath.LastRoom;
        exit = lastRoom.Connections.Find(c => c is Exit exit) as Exit ??
            throw new InvalidOperationException("No valid exits in the last room.");
        position = lastRoom.Area.Center;
        var direction = exit.Direction.IsHorizontal() ?
            exit.Direction.GetOpposite() : Direction.Left;
        var stepsDown = DecorFactory.Steps(direction, Direction.Down);
        stepsDown.Position = position;
        stepsDown.IsVisible = false;
        AddEntity(stepsDown);
    }

    void Door_OnComponentRemoved(object? o, ComponentChangedEventArgs e)
    {

    }
}