using LuckNGold.Visuals;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;

namespace LuckNGold.World.Items;

/// <summary>
/// Factory of loot, tools, weapons and other collectibles that can be picked up.
/// </summary>
static class ItemFactory
{
    public static AnimatedRogueLikeEntity Key(Quality quality)
    {
        var animation = $"{quality}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items);
        key.AllComponents.Add(new UnlockingComponent(quality));
        key.Name = $"{quality} Key";
        return key;
    }
}