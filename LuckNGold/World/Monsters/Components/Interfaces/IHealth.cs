namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It has hit points, can die when they go below zero
/// and can suffer or benefit from transient conditions (like confused or hastened).
/// </summary>
internal interface IHealth
{
    event EventHandler<ValueChangedEventArgs<int>>? HPChanged;

    /// <summary>
    /// Hit points remaining.
    /// </summary>
    int HP { get; }
}