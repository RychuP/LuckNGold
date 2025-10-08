using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It stores <see cref="IWieldable"/> items carried by a monster entity.
/// </summary>
internal interface IGear
{
    Dictionary<Hand, RogueLikeEntity> ItemsWielded { get; }

    /// <summary>
    /// Puts <see cref="IWieldable"/> item in one of the hands.
    /// </summary>
    /// <param name="wieldable">Item to be wielded.</param>
    /// <returns>True if wielding succeded, false otherwise.</returns>
    bool Wield(RogueLikeEntity wieldable);

    /// <summary>
    /// Removes <see cref="IWieldable"/> item from hand and places it in available inventory.
    /// </summary>
    /// <param name="wieldable">Item to be put away.</param>
    /// <returns>True if there was available slot in other inventory 
    /// and item was put away, false otherwise.</returns>
    bool PutAway(RogueLikeEntity wieldable);
}