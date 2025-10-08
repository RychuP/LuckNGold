using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It stores <see cref="IWearable"/> items worn by a monster entity.
/// </summary>
internal interface IOutfit
{
    Dictionary<PartLayerPair, RogueLikeEntity> ItemsWorn { get; }

    /// <summary>
    /// Adds <see cref="IWearable"/> item to items worn.
    /// </summary>
    /// <param name="wearable">Item to be put on.</param>
    /// <returns>True if the entity was added to items worn, false otherwise.</returns>
    bool PutOn(RogueLikeEntity wearable);

    /// <summary>
    /// Removes <see cref="IWearable"/> from items worn and places it in available inventory.
    /// </summary>
    /// <param name="wearable">Item to be taken off.</param>
    /// <returns>True if there was available slot in other inventory 
    /// and item was put away, false otherwise.</returns>
    bool TakeOff(RogueLikeEntity wearable);
}

/// <summary>
/// KeyValue pair composed of <see cref="BodyPart"/> and <see cref="ClothingLayer"/>
/// used in <see cref="IOutfit"/>.
/// </summary>
record PartLayerPair(BodyPart Part, ClothingLayer Layer);