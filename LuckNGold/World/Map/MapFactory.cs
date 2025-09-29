using GoRogue.MapGeneration;
using LuckNGold.Generation;
using LuckNGold.World.Terrain;
using SadRogue.Primitives.GridViews;

namespace LuckNGold.World.Map;

/// <summary>
/// Similar to <see cref="MapObjectFactory"/>, but for producing various types of maps. The functions here
/// use GoRogue map generation and then translate the results to integration library structures.
/// </summary>
/// 
/// <remarks>
/// CUSTOMIZATION: Modify the functions as applicable to generate appropriate maps; both the map data generation
/// (using GoRogue) and the translation to integration library structure occurs here.
///
/// As is the case for map objects, you can use composition to create your objects by attaching components
/// directly to the map. The integration library also supports creating subclasses of RogueLikeMap 
/// (as we do here).
///
/// Additionally, GoRogue's map generation framework supports adding arbitrary components to contexts, 
/// so the integration library map could be added as a component to the context, 
/// and then things like enemy placement could be their own custom GoRogue map generation steps.
/// </remarks>
static class MapFactory
{
    public static GameMap GenerateMap(int width, int height)
    {
        var generator = new Generator(width, height)
            .ConfigAndGenerateSafe(gen =>
            {
                gen.AddStep(new MainPathGenerator(15));
                gen.AddStep(new SidePathGenerator());
                gen.AddStep(new MinorPathGenerator());
            });

        // Create actual integration library map.
        var map = new GameMap(generator.Context);

        // get gridview of the terrain
        var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
        generator.Context.Remove("WallFloor");

        // translate gridview into terrain
        map.ApplyTerrainOverlay(generatedMap, (pos, val) => val ?
            new Floor(pos) : new Wall(pos, generatedMap));

        // Generate 10 enemies, placing them in random walkable locations for demo purposes.
        //for (int i = 0; i < 10; i++)
        //{
        //    var enemy = MapObjectFactory.Enemy();
        //    enemy.Position = GlobalRandom.DefaultRNG.RandomPosition(map.WalkabilityView, true);
        //    map.AddEntity(enemy);
        //}

        return map;
    }
}