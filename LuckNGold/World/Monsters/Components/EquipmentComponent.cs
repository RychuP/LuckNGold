using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can equip items.
/// </summary>
internal class EquipmentComponent : RogueLikeComponentBase<RogueLikeEntity>, IEquipment
{
    public event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? EquipmentChanged;

    readonly Dictionary<EquipSlot, RogueLikeEntity?> _equipment = new()
    {
        { EquipSlot.Head, null },
        { EquipSlot.Body, null },
        { EquipSlot.Feet, null },
        { EquipSlot.LeftHand, null },
        { EquipSlot.RightHand, null },
    };
    public IReadOnlyDictionary<EquipSlot, RogueLikeEntity?> Equipment => _equipment;
    public RogueLikeEntity? Head => Equipment[EquipSlot.Head];
    public RogueLikeEntity? Body => Equipment[EquipSlot.Body];
    public RogueLikeEntity? Feet => Equipment[EquipSlot.Feet];
    public RogueLikeEntity? LeftHand => Equipment[EquipSlot.LeftHand];
    public RogueLikeEntity? RightHand => Equipment[EquipSlot.RightHand];

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

        // Check the entity can be equipped.
        var equippableComponent = item.AllComponents.GetFirstOrDefault<IEquippable>();
        if (equippableComponent is null)
            return false;

        // Get equip slot from the item.
        var slot = equippableComponent.Slot;

        // Check equipped item matches with the given item.
        var equippedItem = Equipment[slot];
        if (equippedItem != item)
            return false;

        // Check parent has an inventory.
        var inventory = Parent.AllComponents.GetFirstOrDefault<IInventory>();
        if (inventory is null)
            return false;

        // Check inventory has space.
        if (inventory.IsFull())
            return false;

        // Unequip item.
        _equipment[slot] = null;
        OnEquipmentChanged(equippedItem, null);

        // Send the item to the inventory.
        inventory.Add(equippedItem);

        return true;
    }

    public bool Unequip(EquipSlot slot)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        // Get equipped item.
        var equippedItem = Equipment[slot];
        if (equippedItem is null) 
            return false;

        // Check parent has an inventory.
        var inventory = Parent.AllComponents.GetFirstOrDefault<IInventory>();
        if (inventory is null)
            return false;

        // Check inventory has space.
        if (inventory.IsFull())
            return false;

        // Unequip item.
        _equipment[slot] = null;
        OnEquipmentChanged(equippedItem, null);

        // Send the item to the inventory.
        inventory.Add(equippedItem);

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