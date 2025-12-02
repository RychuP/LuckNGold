namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can store coins and other means of payment.
/// </summary>
internal interface IWallet
{
    /// <summary>
    /// Notifies when value of <see cref="Coins"/> changes.
    /// </summary>
    event EventHandler<ValueChangedEventArgs<int>>? CoinsChanged;

    /// <summary>
    /// Number of coins currently held in <see cref="IWallet"/>.
    /// </summary>
    int Coins { get; set; }
}