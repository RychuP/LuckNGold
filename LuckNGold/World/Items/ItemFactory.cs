using LuckNGold.Visuals;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

/// <summary>
/// Factory of loot, tools, weapons and other collectibles.
/// </summary>
static class ItemFactory
{
    public static AnimatedRogueLikeEntity Key(Gemstone gemstone)
    {
        var animation = $"{gemstone}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items)
        {
            Name = $"{gemstone} Key"
        };
        key.AllComponents.Add(new UnlockingComponent((Quality)gemstone));
        return key;
    }

    public static AnimatedRogueLikeEntity Coin()
    {
        var coin = new AnimatedRogueLikeEntity("Coin", false, GameMap.Layer.Items)
        {
            Name = "Coin"
        };
        coin.AllComponents.Add(new CollectableComponent(1, 1000));
        return coin;
    }

    public static RogueLikeEntity Onyx() =>
        Gemstone("Onyx", 2, Material.Onyx);

    public static RogueLikeEntity Amber() =>
        Gemstone("Amber", 4, Material.Amber);

    public static RogueLikeEntity Emerald() =>
        Gemstone("Emerald", 8, Material.Emerald);

    public static RogueLikeEntity Ruby() =>
        Gemstone("Ruby", 16, Material.Ruby);

    public static RogueLikeEntity Diamond() =>
        Gemstone("Diamond", 32, Material.Diamond);

    static RogueLikeEntity Gemstone(string name, int value, Material material)
    {
        if (material < Material.Onyx || material > Material.Diamond)
            throw new ArgumentException("Incorrect material for a gemstone entity.");

        var glyphDef = Program.Font.GetGlyphDefinition(name);
        var appearance = glyphDef.CreateColoredGlyph();
        var gemstone = new RogueLikeEntity(appearance, layer: (int)GameMap.Layer.Items)
        {
            Name = name
        };
        gemstone.AllComponents.Add(new ValueComponent(value));
        gemstone.AllComponents.Add(new IngredientComponent(material, 1, 15));
        return gemstone;
    }
}