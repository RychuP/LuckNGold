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

    readonly Dictionary<BodyPart, RogueLikeEntity?> _equipment;
    public IReadOnlyDictionary<BodyPart, RogueLikeEntity?> Equipment => _equipment;

    /// <summary>
    /// Initializes an instance of <see cref="EquipmentComponent"/> class.
    /// </summary>
    public EquipmentComponent() : base(false, false, false, false)
    {
        int slotCount = Enum.GetValues(typeof(BodyPart)).Length;
        _equipment = new(slotCount);
    }

    public bool Equip(RogueLikeEntity item, out RogueLikeEntity? unequippedItem)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent needs to be on the map.");

        if (item.Layer != (int)GameMap.Layer.Items)
            throw new InvalidOperationException("Wearable entity is not an item.");

        unequippedItem = null;

        // Check the entity can be equipped.
        var equippableComponent = item.AllComponents.GetFirstOrDefault<IEquippable>();
        if (equippableComponent is null)
            return false;

        // Unequip items already present in the slot.
        var slot = equippableComponent.Slot;
        if (Equipment.TryGetValue(slot, out RogueLikeEntity? prevItem))
        {
            if (prevItem == item)
                return false;
            else if (prevItem != null)
                _equipment[slot] = null;

            unequippedItem = prevItem;
        }

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

        // Unequip items already present in the slot.
        var slot = equippableComponent.Slot;
        if (Equipment.TryGetValue(slot, out RogueLikeEntity? prevItem))
        {
            if (prevItem != item)
                return false;
            else
            {
                _equipment[slot] = null;
                OnEquipmentChanged(prevItem, null);
            }
        }

        return false;
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