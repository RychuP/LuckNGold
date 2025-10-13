using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that is considered valuable and can be used as a payment method.
/// </summary>
internal class CurrencyComponent(int amount) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ICurrency
{
    public int Amount => amount;
}