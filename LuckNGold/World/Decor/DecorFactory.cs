using LuckNGold.Visuals;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Decor;

static class DecorFactory
{
    public static RogueLikeEntity Flag(Crystal crystal)
    {
        return new AnimatedRogueLikeEntity($"{crystal}Flag", false, GameMap.Layer.Decor)
        {
            Name = $"{crystal} Flag"
        };
    }

    public static RogueLikeEntity Candle(Size size)
    {
        return new AnimatedRogueLikeEntity($"{size}Candle", false, GameMap.Layer.Decor)
        {
            Name = $"{size} Candle",
            IsWalkable = false
        };
    }

    public static RogueLikeEntity Torch()
    {
        return new AnimatedRogueLikeEntity("Torch", false, GameMap.Layer.Decor)
        {
            Name = "Torch"
        };
    }

    public static RogueLikeEntity SideTorch(Direction direction)
    {
        if (!direction.IsHorizontal())
            throw new ArgumentException("Side torch does not accept vertical directions.", 
                nameof(direction));

        return new AnimatedRogueLikeEntity($"SideTorch{direction}", false, GameMap.Layer.Decor)
        {
            Name = "Side Torch"
        };
    }
}