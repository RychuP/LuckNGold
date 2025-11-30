namespace LuckNGold.World.Turns.Actions;

/// <summary>
/// It can happen and takes specific amount of time.
/// </summary>
internal interface IEvent
{
    /// <summary>
    /// Amount of time the <see cref="IEvent"/> takes.
    /// </summary>
    int Time { get; set; }
}