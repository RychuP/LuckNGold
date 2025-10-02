using LuckNGold.Visuals;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;

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
        coin.AllComponents.Add(new TreasureComponent(TreasureType.Coin, 1));
        return coin;
    }
}