using LuckNGold.World.Items.Components.Interfaces;

namespace LuckNGold.World.Monsters.Enums;

/// <summary>
/// Body part slots that can hold <see cref="IEquippable"/> items.
/// </summary>
enum EquipSlot
{
    Head,           // helmets, hoods
    Body,           // clothes, armour (not just torso, but legs and arms too, hence 'body')
    LeftHand,       // shields
    RightHand,      // single and two hand weapons
    Feet            // shoes, boots
}