using LuckNGold.World.Furniture.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that can drop loot.
/// </summary>
/// <param name="contents"></param>
class LootSpawnerComponent(params RogueLikeEntity[] contents) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ILootSpawner
{
    public RogueLikeEntity[] Contents { get; } = contents;

    public void DropItems()
    {
        
    }
}