using LuckNGold.Visuals;
using LuckNGold.World.Items.Components;

namespace LuckNGold.World.Items;

static class ItemFactory
{
    public static AnimatedRogueLikeEntity SilverKey() =>
        Key(KeyType.Silver);

    public static AnimatedRogueLikeEntity GoldenKey() =>
        Key(KeyType.Golden);

    static AnimatedRogueLikeEntity Key(KeyType type)
    {
        var animation = $"{type}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items);
        key.AllComponents.Add(new KeyComponent(type));
        key.Name = $"{type} Key";
        return key;
    }
}