namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can change its appearance depending on motion.
/// </summary>
internal interface IMotionAppearance
{
    void UpdateAppearance(Direction direction);
}