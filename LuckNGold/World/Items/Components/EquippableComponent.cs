using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be equipped.
/// </summary>
/// <param name="slot">Slot where the item can be placed.</param>
internal class EquippableComponent(EquipSlot slot) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IEquippable
{
    public EquipSlot Slot => slot;
}