namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can gain experience and improve its level.
/// </summary>
internal interface ILevel
{
    event EventHandler<ValueChangedEventArgs<int>>? ExperienceChanged;
    event EventHandler<ValueChangedEventArgs<int>>? LevelChanged;

    /// <summary>
    /// Experience gained affecting the level.
    /// </summary>
    int Experience { get; }

    /// <summary>
    /// Level gained affecting the stats.
    /// </summary>
    int Level { get; }
}