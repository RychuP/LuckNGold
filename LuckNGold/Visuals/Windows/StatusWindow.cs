using LuckNGold.World.Monsters.Components;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Window that displays player health, coins and other at-a-glance information.
/// </summary>
internal class StatusWindow : ScreenSurface
{
    readonly int _coinsTextX;
    readonly string _coinsText = "Coins: ";

    public StatusWindow(WalletComponent wallet) : base(Program.Width, 1)
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