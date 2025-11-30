namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can track time points used to perform actions.
/// </summary>
internal interface ITimeTracker
{
    /// <summary>
    /// Amount of time points gained per turn.
    /// </summary>
    int Speed { get; set; }

    /// <summary>
    /// Time remaining to be used in the current turn.
    /// </summary>
    int Time { get; set; }
}