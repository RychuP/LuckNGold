using SadRogue.Integration;

namespace LuckNGold.World.Furniture.Interfaces;

/// <summary>
/// It can spawn item entities.
/// </summary>
internal interface ILootSpawner
{
    List<RogueLikeEntity> Contents { get; }
    void DropItems();
}