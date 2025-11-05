namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can change appearance of an entity based on its movement direction.
/// </summary>
internal interface IMotion
{
    ColoredGlyph[] Appearances { get; }
    void UpdateAppearance(Direction direction);
}