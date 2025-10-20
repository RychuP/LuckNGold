using LuckNGold.Generation.Items;
using LuckNGold.World.Items;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates item data objects from generator to RogueLike entities.
partial class GameMap
{
    public void PlaceItem(Item item)
    {
        if (item.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(item.Position);
        if (entity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        entity = CreateItem(item);
        AddEntity(entity, item.Position);
    }

    /// <summary>
    /// Converts data object into a <see cref="RogueLikeEntity"/> that can be placed on the map.
    /// </summary>
    /// <param name="item">Data object from generator.</param>
    /// <returns>An instance of <see cref="RogueLikeEntity"/> 
    /// created from given data object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static RogueLikeEntity CreateItem(Item item) =>
        item is Key key ? ItemFactory.Key(key.Material) :
        item is Coin ? ItemFactory.Coin() :
        throw new ArgumentException("Item not implemented.");
}