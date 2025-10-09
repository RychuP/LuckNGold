using LuckNGold.World.Items.Components;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// It represents an outfit worn by a monster entity.
/// </summary>
internal class OutfitComponent() : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IOutfit
{
    public event EventHandler? ItemPutOn;
    public event EventHandler? ItemTakenOff;

    public Dictionary<PartLayerPair, RogueLikeEntity> ItemsWorn { get; } = [];

    /// <inheritdoc/>
    public bool PutOn(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component has to be attached to an entity.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Parent has to be on the map.");

        if (item.Layer != (int)GameMap.Layer.Items)
            throw new InvalidOperationException("Wearable entity is not an item.");

        // Check the entity can be worn
        var wearable = item.AllComponents.GetFirstOrDefault<WearableComponent>();
        if (wearable is null) 
            return false;

        // Place the item in its slot in the outfit
        var placement = new PartLayerPair(wearable.BodyPart, wearable.Layer);
        if (ItemsWorn.TryGetValue(placement, out RogueLikeEntity? prevItem))
        {
            // Check the prev item in the outfit slot is not the same as new entity
            if (prevItem == item)
                return false;

            // Replace item worn
            ItemsWorn[placement] = item;
            TakeOff(prevItem);
        }
        else
            ItemsWorn.Add(placement, item);

        // Fire the event
        OnItemPutOn(item);
        return true;
    }

    /// <inheritdoc/>
    public bool TakeOff(RogueLikeEntity wearable)
    {
        throw new NotImplementedException();
    }

    void OnItemPutOn(RogueLikeEntity item)
    {
        ItemPutOn?.Invoke(this, EventArgs.Empty);
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (Parent!.Layer != (int)GameMap.Layer.Monsters)
            throw new InvalidOperationException("Component is meant to be added " +
                "to a Monster entity.");
    }
}