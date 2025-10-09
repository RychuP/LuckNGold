namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It is accepted by traders like currency.
/// </summary>
internal interface IValuable
{
    int Value { get; }
}