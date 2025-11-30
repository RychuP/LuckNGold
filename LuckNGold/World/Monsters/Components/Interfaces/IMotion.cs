namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can move (change position).
/// </summary>
internal interface IMotion
{
    /// <summary>
    /// Evaluates motion cost in time units.
    /// </summary>
    int GetMoveCost();
}