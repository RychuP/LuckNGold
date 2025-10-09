using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be accepted by traders like currency.
/// </summary>
internal class ValueComponent(int amount) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IValuable
{
    public int Value => amount;
}
