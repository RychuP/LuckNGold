using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can equip items.
/// </summary>
internal class EquipmentComponent : RogueLikeComponentBase<RogueLikeEntity>, IEquipment
{
    public event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? EquipmentChanged;

    readonly Dictionary<BodyPart, RogueLikeEntity?> _equipment = new()
    {
        { BodyPart.Head, null },
        { BodyPart.Torso, null },
        { BodyPart.Feet, null },
        { BodyPart.LeftHand, null },
        { BodyPart.RightHand, null },
    };
    public IReadOnlyDictionary<BodyPart, RogueLikeEntity?> Equipment => _equipment;
    public RogueLikeEntity? Head => Equipment[BodyPart.Head];
    public RogueLikeEntity? Torso => Equipment[BodyPart.Torso];
    public RogueLikeEntity? Feet => Equipment[BodyPart.Feet];
    public RogueLikeEntity? LeftHand => Equipment[BodyPart.LeftHand];
    public RogueLikeEntity? RightHand => Equipment[BodyPart.RightHand];

    /// <summary>
    /// Initializes an instance of <see cref="EquipmentComponent"/> class.
    /// </summary>
    public EquipmentComponent() : base(false, false, false, false)
    {

    }

    public bool Equip(RogueLikeEntity item, out RogueLikeEntity? prevItem)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        if (item.Layer != (int)GameMap.Layer.Items)
            throw new InvalidOperationException("Wearable entity is not an item.");

        prevItem = null;

        // Check the entity can be equipped.
        var equippableComponent = item.AllComponents.GetFirstOrDefault<IEquippable>();
        if (equippableComponent is null)
            return false;

        // Get item that is already present in the slot.
        var slot = equippableComponent.Slot;
        prevItem = Equipment[slot];
        if (prevItem == item)
            return false;

        // Place item in its equipement slot.
        _equipment[slot] = item;
        
        // Fire the event
        OnEquipmentChanged(prevItem, item);
        return true;
    }

    public bool Unequip(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        if (item.Layer != (int)GameMap.Layer.Items)
            throw new InvalidOperationException("Wearable entity is not an item.");

        // Check the entity can be equipped.
        var equippableComponent = item.AllComponents.GetFirstOrDefault<IEquippable>();
        if (equippableComponent is null)
            return false;

        // Check item is currently equipped.
        var slot = equippableComponent.Slot;
        var prevItem = Equipment[slot];
        if (prevItem != item)
            return false;

        // Unequip the item.
        _equipment[slot] = null;
        OnEquipmentChanged(prevItem, null);
        return true;
    }

    void OnEquipmentChanged(RogueLikeEntity? prevItem, RogueLikeEntity? newItem)
    {
        var args = new ValueChangedEventArgs<RogueLikeEntity?>(prevItem, newItem);
        EquipmentChanged?.Invoke(this, args);
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (Parent!.Layer != (int)GameMap.Layer.Monsters)
            throw new InvalidOperationException("Component is meant to be added " +
                "to a Monster entity.");
    }
}