using GoRogue.Random;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Map;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using ShaiRandom.Generators;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that can drop loot.
/// </summary>
class LootSpawnerComponent : RogueLikeComponentBase<RogueLikeEntity>, ILootSpawner
{
    public List<RogueLikeEntity> Contents { get; }

    public LootSpawnerComponent(List<RogueLikeEntity> contents) 
        :base(false, false, false, false)
    {
        if (contents.Where(e => e.Layer != (int)GameMap.Layer.Items).Any())
            throw new ArgumentException("Only item entities are allowed to be loot.");

        Contents = contents;
    }

    public void DropItems()
    {
        if (Contents.Count == 0)
            return;

        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is not GameMap map)
            throw new InvalidOperationException("Parent entity has to be on the map.");

        // Get points surrounding parent entity.
        var neighbours = Program.Adjacency.Neighbors(Parent.Position);

        // Create a list for valid spawn positions.
        List<Point> spawnPositions = [];

        // Sieve through neighbour positions to find valid spawn cells.
        foreach (var point in neighbours)
        {
            if (!map.GetTerrainAt<RogueLikeCell>(point)!.IsWalkable)
                continue;

            var entities = map.GetEntitiesAt<RogueLikeEntity>(point);
            bool cellCanAcceptLoot = true;

            foreach (var entity in entities)
            {
                if (entity.Layer <= (int)GameMap.Layer.Items)
                {
                    cellCanAcceptLoot = false;
                    break;
                }
            }

            if (cellCanAcceptLoot)
                spawnPositions.Add(point);
        }

        var rnd = GlobalRandom.DefaultRNG;
        List<RogueLikeEntity> toBeRemoved = [];

        // Place loot in random valid positions around the parent entity.
        foreach (var item in Contents)
        {
            if (item.CurrentMap != null)
                throw new InvalidOperationException("Loot item should not be on the map yet.");

            // Break from dropping loot when no empty spaces are available
            if (spawnPositions.Count == 0)
                break;

            var index = rnd.RandomIndex(spawnPositions);
            item.Position = spawnPositions[index];
            map.AddEntity(item);
            toBeRemoved.Add(item);
            spawnPositions.RemoveAt(index);
        }

        // Remove spawned loot from spawner contents.
        foreach (var item in toBeRemoved)
            Contents.Remove(item);
    }
}