using GoRogue.GameFramework;

namespace LuckNGold.World;

internal abstract class Terrain(Point position, Color lightColor, Color darkColor, bool isWalkable, bool isTransparent) 
    : GameObject(position, (int)EntityLayer.Terrain, isWalkable, isTransparent)
{
    public Color LightColor { get; init; } = lightColor;
    public Color DarkColor { get; init; } = darkColor;
}