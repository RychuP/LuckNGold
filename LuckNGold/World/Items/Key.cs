using LuckNGold.Visuals;

namespace LuckNGold.World.Items;

internal class Key(Point position, string color) : 
    AnimatedRogueLikeEntity(position, $"{color}Key", true, GameMap.Layer.Items)
{ }