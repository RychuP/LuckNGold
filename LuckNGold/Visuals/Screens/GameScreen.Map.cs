using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.World.Furnitures.Components;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Terrain;
using SadRogue.Integration;
using SadRogue.Primitives.GridViews;
using SadRogue.Primitives.SpatialMaps;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    /// <summary>
    /// Map that displays the game world.
    /// </summary>
    public GameMap Map { get; }

    /// <summary>
    /// Opening components that need to be closed when player moves.
    /// </summary>
    readonly List<OpeningComponent> _openings = [];

    public GameMap GenerateMap(int width, int height, int mainPathRoomCount)
    {
        var generator = new Generator(width, height)
            .ConfigAndGenerateSafe(gen =>
            {
                gen.AddStep(new MainPathGenerator(mainPathRoomCount));
                gen.AddStep(new SidePathGenerator());
                gen.AddStep(new MinorPathGenerator());
                gen.AddStep(new SectionGenerator());
                gen.AddStep(new ObjectiveGenerator());
                gen.AddStep(new FirstRoomGenerator());
                gen.AddStep(new DecorGenerator());
            });

        // Create actual integration library map.
        var map = new GameMap(generator.Context);
        map.ObjectAdded += Map_OnObjectAdded;
        map.ObjectRemoved += Map_OnObjectRemoved;

        // Get gridview of the terrain.
        var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
        generator.Context.Remove("WallFloor");

        // Translate gridview into terrain.
        map.ApplyTerrainOverlay(generatedMap, (pos, val) => val ?
            new Floor(pos) : new Wall(pos, generatedMap));

        // Place entities on the map.
        var rooms = generator.Context.GetFirst<ItemList<Room>>("Rooms").Items;
        int gateCount = 0;
        foreach (var room in rooms)
        {
            foreach (var entity in room.Contents)
            {
                if (entity is Decor decor)
                    map.PlaceDecor(decor);
                else if (entity is Furniture furniture)
                {
                    map.PlaceFurniture(furniture);
                    if (DebugEnabled && furniture is Gate)
                        gateCount++;
                }
                else if (entity is Item item)
                    map.PlaceItem(item);
            }
        }
        if (DebugEnabled)
            Print($"Gate count: {gateCount}");

        return map;
    }

    void Map_OnObjectAdded(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is not RogueLikeEntity entity) return;

        if (entity.Name == "Door")
        {
            // Recalculate FOV when transparency of the door changes.
            entity.TransparencyChanged += RogueLikeEntity_OnTransparencyChanged;
        }
        else if (entity.Name == "Chest")
        {
            // Save the opening component to the list of openings that need to be closed.
            var openingComponent = entity.AllComponents.GetFirst<OpeningComponent>();
            _openings.Add(openingComponent);
        }
    }

    void Map_OnObjectRemoved(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is not RogueLikeEntity entity) return;

        if (entity.Name == "Door")
        {
            entity.TransparencyChanged -= RogueLikeEntity_OnTransparencyChanged;
        }
        else if (entity.Name == "Chest")
        {
            var openingComponent = entity.AllComponents.GetFirst<OpeningComponent>();
            _openings.Remove(openingComponent);
        }
    }

    // Recalculates player FOV if an entity that changed its transparency is in view.
    void RogueLikeEntity_OnTransparencyChanged(object? o, ValueChangedEventArgs<bool> e)
    {
        if (o is not RogueLikeEntity door || door.Name != "Door")
            return;

        var playerFOV = Player.AllComponents.GetFirst<PlayerFOVController>();
        if (Map.PlayerFOV.CurrentFOV.Contains(door.Position))
            playerFOV.CalculateFOV();
    }
}