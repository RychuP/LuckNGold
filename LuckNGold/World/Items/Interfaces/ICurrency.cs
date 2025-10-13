namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be used as a payment method.
/// </summary>
internal interface ICurrency : ICarryable
{
    int Amount { get; }
}