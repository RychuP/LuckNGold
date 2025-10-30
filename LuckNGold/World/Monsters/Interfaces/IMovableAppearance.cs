namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can change its appearance depending on motion.
/// </summary>
internal interface IMovableAppearance
{
    void UpdateAppearance(Direction direction);
}