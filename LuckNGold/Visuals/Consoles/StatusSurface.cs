using LuckNGold.Config;
using LuckNGold.World.Monsters.Components;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Surface that displays player health, coins and other at-a-glance information.
/// </summary>
internal class StatusSurface : ScreenSurface
{
    readonly int _coinsTextX;
    readonly string _coinsText = "Coins: ";

    /// <summary>
    /// Initializes an instance of <see cref="StatusSurface"/> class.
    /// </summary>
    /// <param name="wallet">Source component for the surface.</param>
    public StatusSurface(WalletComponent wallet) : base(GameSettings.Width, 1)
    {
        wallet.CoinsChanged += WalletComponent_OnCoinsChanged;
        _coinsTextX = (Width - _coinsText.Length - 1) / 2;
        PrintCoinsText(wallet.Coins);
    }

    void WalletComponent_OnCoinsChanged(object? o, ValueChangedEventArgs<int> e)
    {
        PrintCoinsText(e.NewValue);
    }

    void PrintCoinsText(int amount)
    {
        string text = $"{_coinsText}[c:r f:Yellow]{amount}";
        ColoredString coinsText = ColoredString.Parser.Parse(text);
        Surface.Print(_coinsTextX, 0, coinsText);
    }
}