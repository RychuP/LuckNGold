using LuckNGold.Primitives;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Map;

namespace LuckNGold.World.Items;

static class ToolFactory
{
    public static AnimatedRogueLikeEntity Key(IGemstone gemstone)
    {
        var animation = $"{gemstone}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items)
        {
            Name = $"{gemstone} Key"
        };
        key.AllComponents.Add(new UnlockingComponent((Quality)gemstone.GemstoneType));
        return key;
    }
}