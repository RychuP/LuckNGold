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
using LuckNGold.World.Monsters.Interfaces;
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
        int doubleGateCount = 0;
        foreach (var room in rooms)
        {
            foreach (var entity in room.Contents)
            {
                if (entity is Decor decor)
                    map.PlaceDecor(decor);
                else if (entity is Furniture furniture)
                {
                    map.PlaceFurniture(furniture);
                    if (DebugEnabled && furniture is Gate gate)
                    {
                        gateCount++;
                        if (gate.IsDouble)
                            doubleGateCount++;
                    }
                }
                else if (entity is Item item)
                    map.PlaceItem(item);
            }
        }
        if (DebugEnabled)
            Print($"Gate count: {gateCount}, of which double: {doubleGateCount}");

        return map;
    }

    /// <summary>
    /// Adds event handlers to entities.
    /// </summary>
    void Map_OnObjectAdded(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is not RogueLikeEntity entity) return;

        if (entity.Name.Contains("Door"))
        {
            // Recalculate FOV when transparency of the door changes.
            entity.TransparencyChanged += RogueLikeEntity_OnTransparencyChanged;
        }
        else if (entity.Name.Contains("Chest"))
        {
            // Save the opening component to the list of openings that need to be closed.
            var openingComponent = entity.AllComponents.GetFirst<OpeningComponent>();
            _openings.Add(openingComponent);
        }

        // Monitor visibility of the monster to show its onion appearance in monster layer.
        if (entity.Layer == (int)GameMap.Layer.Monsters &&
            entity.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
        {
            entity.IsVisibleChanged += Monster_OnIsVisibleChanged;
            onionComponent.CurrentFrameChanged += OnionComponent_OnCurrentFrameChanged;
        }
    }

    /// <summary>
    /// Removes event handlers from entities.
    /// </summary>
    void Map_OnObjectRemoved(object? o, ItemEventArgs<IGameObject> e)
    {
        if (e.Item is not RogueLikeEntity entity) return;

        if (entity.Name.Contains("Door"))
        {
            entity.TransparencyChanged -= RogueLikeEntity_OnTransparencyChanged;
        }
        else if (entity.Name.Contains("Chest"))
        {
            var openingComponent = entity.AllComponents.GetFirst<OpeningComponent>();
            _openings.Remove(openingComponent);
        }

        if (entity.Layer == (int)GameMap.Layer.Monsters &&
            entity.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
        {
            entity.IsVisibleChanged -= Monster_OnIsVisibleChanged;
            onionComponent.CurrentFrameChanged -= OnionComponent_OnCurrentFrameChanged;
        }
    }

    /// <summary>
    /// Reacts to map zoom changes.
    /// </summary>
    void Map_OnViewZoomChanged(object? o, EventArgs e)
    {
        if (_entityInfoWindow.IsVisible)
            HideEntityInfo();

        var visibleMonsters = Map.Monsters
            .Where(m => m.IsVisible);

        // Update font size of all visible monsters.
        foreach (var monster in visibleMonsters)
        {
            if (monster.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
                onionComponent.SetFontSize(Map.FontSizeMultiplier);
        }
    }

    /// <summary>
    /// Recalculates player FOV if an entity that changed its transparency is in view.
    /// </summary>
    void RogueLikeEntity_OnTransparencyChanged(object? o, ValueChangedEventArgs<bool> e)
    {
        if (o is not RogueLikeEntity door || door.Name != "Door")
            return;

        var playerFOV = Player.AllComponents.GetFirst<PlayerFOVController>();
        if (Map.PlayerFOV.CurrentFOV.Contains(door.Position))
            playerFOV.CalculateFOV();
    }

    /// <summary>
    /// Adds or removes monster onion appearance from gamescreen monster layer 
    /// when monster visibility changes.
    /// </summary>
    void Monster_OnIsVisibleChanged(object? o, EventArgs e)
    {
        if (o is not RogueLikeEntity monster) return;

        if (monster.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
        {
            if (monster.IsVisible)
            {
                _monsterLayer.Children.Add(onionComponent.CurrentFrame);

                // Update font size if necessary.
                if (onionComponent.FontSizeMultiplier != Map.FontSizeMultiplier)
                    onionComponent.SetFontSize(Map.FontSizeMultiplier);
            }
            else
            {
                Children.Remove(onionComponent.CurrentFrame);
            }
        }
    }

    /// <summary>
    /// Replaces monster onion appearance in the gamescreen monster layer.
    /// </summary>
    void OnionComponent_OnCurrentFrameChanged(object? o, ValueChangedEventArgs<ILayerStack> e)
    {
        if (o is not OnionComponent onionComponent) return;
        if (onionComponent.Parent is null) return;

        _monsterLayer.Children.Remove(e.OldValue);
        _monsterLayer.Children.Add(e.NewValue);
        var viewPosition = Map.DefaultRenderer!.Surface.ViewPosition;
        e.NewValue.Position = onionComponent.Parent.Position - viewPosition;
    }

    void Monster_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        if (o is not RogueLikeEntity monster) return;
    }
}