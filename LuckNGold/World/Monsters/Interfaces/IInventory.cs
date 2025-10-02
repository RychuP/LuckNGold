using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can store entities with <see cref="ICarryable"/> component.
/// </summary>
internal interface IInventory
{
    /// <summary>
    /// Maximum amount of entities that can be added to <see cref="Items"/>.
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// List of all entities with <see cref="ICarryable"/> component held in the inventory.
    /// </summary>
    List<RogueLikeEntity> Items { get; }

    /// <summary>
    /// Monetary value of all entities with <see cref="ITreasure"/> held in the inventory.
    /// </summary>
    int Value { get; }

    /// <summary>
    /// List of all entities with <see cref="ITreasure"/> component held in the inventory.
    /// </summary>
    List<RogueLikeEntity> Treasure { get; }

    /// <summary>
    /// Removes an entity with <see cref="ICarryable"/> from the inventory.
    /// </summary>
    /// <param name="item">Entity with <see cref="ICarryable"/> being removed.</param>
    /// <returns>True if the entity was removed from the inventory, false otherwise.</returns>
    bool Remove(RogueLikeEntity item);

    /// <summary>
    /// Adds an entity with <see cref="ICarryable"/> to the contents of the inventory.
    /// </summary>
    /// <param name="item">Entity with <see cref="ICarryable"/> being added.</param>
    /// <returns>True if entity was added to the inventory, false otherwise.</returns>
    bool Add(RogueLikeEntity item);
}