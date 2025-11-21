using LuckNGold.World.Items.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be purchased and sold by by traders.
/// </summary>
internal class ValueComponent(int amount) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IValuable
{
    public int Value => amount;
}
