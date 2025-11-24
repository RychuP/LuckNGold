using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can hold entities with <see cref="IEquippable"/> in <see cref="EquipSlot"/> slots.
/// </summary>
internal interface IEquipment
{
    event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? EquipmentChanged;

    IReadOnlyDictionary<EquipSlot, RogueLikeEntity?> Equipment { get; }

    bool Equip(RogueLikeEntity item, out RogueLikeEntity? unequippedItem);

    bool Unequip(RogueLikeEntity item);

    bool Unequip(EquipSlot equipSlot);

    RogueLikeEntity? Head { get; }
    RogueLikeEntity? Body { get; }
    RogueLikeEntity? Feet { get; }
    RogueLikeEntity? LeftHand { get; }
    RogueLikeEntity? RightHand { get; }
}