using GoRogue.Random;
using LuckNGold.Visuals;
using LuckNGold.World.Map;
using SadRogue.Integration;
using ShaiRandom.Generators;

namespace LuckNGold.World.Terrain;

internal class Floor : RogueLikeCell
{
    // area of the font grid where the floor decals are
    readonly Rectangle _floorDecals = new(6, 0, 4, 4);

    public Floor(Point position) :
        base(position, Color.White, Colors.Floor, 0, (int) GameMap.Layer.Terrain)
    {
        // allocate a random decal to the floor
        var glyphPos = GlobalRandom.DefaultRNG.RandomPosition(_floorDecals);
        Appearance.Glyph = glyphPos.ToIndex(10);
    }
}