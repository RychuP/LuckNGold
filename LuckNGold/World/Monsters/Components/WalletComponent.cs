using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component representing a wallet that can store coins and other means of payment.
/// </summary>
internal class WalletComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IWallet
{
    /// <summary>
    /// Fired when number of <see cref="Coins"/> held changes.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<int>>? CoinsChanged;

    int _coins;
    public int Coins
    {
        get => _coins;
        set
        {
            if (value < 0)
                throw new ArgumentException("Coins cannot be negative.");
            
            var prevVal = _coins;
            _coins = value;

            OnCoinsChanged(prevVal, value);
        }
    }

    void OnCoinsChanged(int prevVal, int newVal)
    {
        var args = new ValueChangedEventArgs<int>(prevVal, newVal);
        CoinsChanged?.Invoke(this, args);
    }
}