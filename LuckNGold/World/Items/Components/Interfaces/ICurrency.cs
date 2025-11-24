using LuckNGold.World.Monsters.Components.Interfaces;

namespace LuckNGold.World.Items.Components.Interfaces;

/// <summary>
/// It can be used as a payment method.
/// </summary>
internal interface ICurrency : ICarryable
{
    /// <summary>
    /// Amount of <see cref="ICurrency"/> added to <see cref="IWallet"/> when entity is collected.
    /// </summary>
    int Amount { get; }
}