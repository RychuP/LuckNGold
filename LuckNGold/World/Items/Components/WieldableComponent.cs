using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be wielded like a tool, weapon or a shield.
/// </summary>
/// <param name="hand">Hands needed to wield the item.</param>
internal class WieldableComponent(Hand hand) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IWieldable
{
    public Hand Hand { get => hand; }
}