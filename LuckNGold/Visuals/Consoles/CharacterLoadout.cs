using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Displays player loadout (equipment in use).
/// </summary>
/// <remarks>The <see cref="CharacterLoadout"/> class provides a visual interface for equipping 
/// items to different body parts. It initializes with predefined slots for head, body, feet, 
/// left hand, and right hand, each represented by a <see cref="Slot"/> object.</remarks>
internal class CharacterLoadout : SlotHolder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterLoadout"/> class.
    /// </summary>
    public CharacterLoadout(IEquipment equipmentComponent) : base(7, 3, 3)
    {
        equipmentComponent.EquipmentChanged += IEquipment_OnEquipmentChanged;

        AddSlot(EquipSlot.Head, GetTranslatedPosition(1, 0), GetPlaceholder("Head"), equipmentComponent);
        AddSlot(EquipSlot.Body, GetTranslatedPosition(1, 1), GetPlaceholder("Torso"), equipmentComponent);
        AddSlot(EquipSlot.Feet, GetTranslatedPosition(1, 2), GetPlaceholder("Feet"), equipmentComponent);
        AddSlot(EquipSlot.LeftHand, GetTranslatedPosition(2, 1), GetPlaceholder("Shield"), equipmentComponent);
        AddSlot(EquipSlot.RightHand, GetTranslatedPosition(0, 1), GetPlaceholder("Weapon"), equipmentComponent);
    }

    public IEnumerable<Slot> Slots => Children
        .Cast<Slot>();

    static ColoredGlyph GetPlaceholder(string placeholderName)
    {
        var glyphDef = Program.Font.GetGlyphDefinition(placeholderName);
        return glyphDef.CreateColoredGlyph();
    }

    void AddSlot(EquipSlot equipSlot, Point position, ColoredGlyph placeHolder, 
        IEquipment equipmentComponent)
    {
        // Create a slot.
        var slot = new Slot(SlotSize, $"{equipSlot}", placeHolder)
        {
            Position = position
        };

        // Show item if any.
        if (equipmentComponent.Equipment[equipSlot] is RogueLikeEntity item)
            slot.ShowItem(item);

        // Add slot to Children.
        Children.Add(slot);
    }

    void IEquipment_OnEquipmentChanged(object? o, ValueChangedEventArgs<RogueLikeEntity?> e)
    {
        if (e.NewValue is RogueLikeEntity newItem)
        {
            var slot = GetSlot(newItem);
            slot.ShowItem(newItem);
        }
        else if (e.OldValue is RogueLikeEntity prevItem)
        {
            var slot = GetSlot(prevItem);
            slot.EraseItem();
        }
        else
        {
            throw new InvalidOperationException("Event with two nulls as new and old value.");
        }
    }

    Slot GetSlot(RogueLikeEntity item)
    {
        var equippable = item.AllComponents.GetFirst<IEquippable>();
        var equipSlot = equippable.Slot;

        return Children
            .Cast<Slot>()
            .Where(s => s.Name == $"{equipSlot}")
            .First();
    }
}