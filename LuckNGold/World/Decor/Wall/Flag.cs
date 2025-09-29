using LuckNGold.Visuals;
using LuckNGold.World.Map;

namespace LuckNGold.World.Decor.Wall;

internal class Flag(Point position, string color) 
    : AnimatedRogueLikeEntity(position, $"{color}Flag", false, GameMap.Layer.Decor)
{
    public string Color { get; } = color;
}