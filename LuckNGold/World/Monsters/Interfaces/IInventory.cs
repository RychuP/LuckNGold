using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// Interface for components that can be treated as inventory to store items.
/// </summary>
internal interface IInventory
{
    int Capacity { get; }
    List<RogueLikeEntity> Items { get; }
    bool Remove(RogueLikeEntity item);
    bool Add(RogueLikeEntity item);
}