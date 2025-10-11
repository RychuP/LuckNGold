namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It is accepted by traders like currency.
/// </summary>
internal interface IValuable
{
    /// <summary>
    /// Monetary value in coins.
    /// </summary>
    int Value { get; }
}