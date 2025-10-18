using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.World.Furniture.Components;
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

        // Get generated entities.
        var decor = generator.Context.GetFirst<ItemList<Entity>>("Decor").Items;
        var furniture = generator.Context.GetFirst<ItemList<Entity>>("Furniture").Items;
        var items = generator.Context.GetFirst<ItemList<Item>>("Items").Items;

        // Place generated entities on the map.
        map.PlaceDecor(decor);
        map.PlaceFurniture(furniture);
        map.PlaceItems(items);

        return map;
    }

    void Map_OnObjectAdded(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is not RogueLikeEntity entity) return;

        if (entity.Name == "Door")
        {
            // Recalculates FOV when transparency of the door changes.
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

    // Recalculates player FOV if an entity that changed its transparency is in view
    void RogueLikeEntity_OnTransparencyChanged(object? o, ValueChangedEventArgs<bool> e)
    {
        if (o is not RogueLikeEntity door || door.Name != "Door")
            return;

        var playerFOV = Player.AllComponents.GetFirst<PlayerFOVController>();
        if (Map.PlayerFOV.CurrentFOV.Contains(door.Position))
            playerFOV.CalculateFOV();
    }
}