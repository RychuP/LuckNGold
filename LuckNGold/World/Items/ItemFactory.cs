using LuckNGold.Visuals;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items;

/// <summary>
/// Factory of loot, tools, weapons and other collectibles that can be picked up.
/// </summary>
static class ItemFactory
{
    public static AnimatedRogueLikeEntity BronzeKey() =>
        Key(KeyColor.Bronze);

    public static AnimatedRogueLikeEntity SilverKey() =>
        Key(KeyColor.Silver);

    public static AnimatedRogueLikeEntity GoldenKey() =>
        Key(KeyColor.Golden);

    static AnimatedRogueLikeEntity Key(KeyColor keyColor)
    {
        var animation = $"{keyColor}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items);
        key.AllComponents.Add(new KeyComponent(keyColor));
        key.Name = $"{keyColor} Key";
        return key;
    }
}