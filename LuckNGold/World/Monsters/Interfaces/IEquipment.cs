using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can hold entities with <see cref="IEquippable"/> in <see cref="BodyPart"/> slots.
/// </summary>
internal interface IEquipment
{
    event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? EquipmentChanged;

    IReadOnlyDictionary<BodyPart, RogueLikeEntity?> Equipment { get; }

    bool Equip(RogueLikeEntity item, out RogueLikeEntity? unequippedItem);

    bool Unequip(RogueLikeEntity item);
}