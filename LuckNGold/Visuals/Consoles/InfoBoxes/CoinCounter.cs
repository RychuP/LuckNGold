using LuckNGold.Resources;
using LuckNGold.World.Monsters.Components.Interfaces;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays number of coins collected so far.
/// </summary>
internal class CoinCounter : InfoBox
{
    public CoinCounter(IWallet wallet) : base("Coins", Strings.CoinCounterDescription)
    {
        wallet.CoinsChanged += (o, e) => Print(e.NewValue);
        Print(wallet.Coins);
    }
}