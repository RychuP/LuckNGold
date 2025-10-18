using LuckNGold.Generation.Items;
using LuckNGold.World.Items;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

partial class GameMap
{
    public void PlaceItems(IReadOnlyList<Item> items)
    {
        foreach (var item in items)
        {
            if (item.Position == Point.None)
                throw new InvalidOperationException("All entities in the list should have " +
                    "a valid position.");

            RogueLikeEntity entity = GetItem(item);
            entity.Position = item.Position;
            AddEntity(entity);
        }
    }
    
    public static RogueLikeEntity GetItem(Item item)
    {
        if (item is Key key)
            return ItemFactory.Key(key.Material);
        else if (item is Coin)
            return ItemFactory.Coin();

        throw new ArgumentException("Item not implemented.");
    }
}