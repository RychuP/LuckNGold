using GoRogue.Random;
using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Map;
using SadConsole.Components;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using ShaiRandom.Generators;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that can drop loot.
/// </summary>
class LootSpawnerComponent : RogueLikeComponentBase<RogueLikeEntity>, ILootSpawner
{
    public List<RogueLikeEntity> Contents { get; }

    public LootSpawnerComponent(params RogueLikeEntity[] contents) 
        :base(false, false, false, false)
    {
        if (contents.Where(e => e.Layer != (int)GameMap.Layer.Items).Any())
            throw new ArgumentException("Only item entities are allowed to be loot.");

        Contents = [.. contents];
    }

    public void DropItems()
    {
        if (Contents.Count == 0)
            return;

        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture or below has to be on the map.");

        // Get surrounding points
        var neighbours = Program.Adjacency.Neighbors(Parent.Position);
        var freeNeighbours = neighbours.Where(p =>
        {
            return !Parent.CurrentMap!.GetEntitiesAt<RogueLikeEntity>(p).Any();
            //var entities = Parent.CurrentMap!.GetEntitiesAt<RogueLikeEntity>(p);
            //return entities.Where(e => e.AllComponents.GetFirstOrDefault<ICarryable>() 
            //    != null).Any();
        }).ToList();

        var rnd = GlobalRandom.DefaultRNG;
        List<RogueLikeEntity> toBeRemoved = [];
        foreach (var item in Contents)
        {
            if (item.CurrentMap != null)
                throw new InvalidOperationException("Loot item should not be on the map yet.");

            // Break from dropping loot when no empty spaces are available
            if (freeNeighbours.Count == 0)
                break;

            var index = rnd.RandomIndex(freeNeighbours);
            item.Position = freeNeighbours[index];
            Parent.CurrentMap!.AddEntity(item);
            toBeRemoved.Add(item);
            freeNeighbours.RemoveAt(index);
        }

        // Remove spawned items from loot spawner contents
        foreach (var item in toBeRemoved)
            Contents.Remove(item);
    }
}