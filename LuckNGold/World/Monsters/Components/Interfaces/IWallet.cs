namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can store coins and other means of payment.
/// </summary>
internal interface IWallet
{
    int Coins { get; set; }
}