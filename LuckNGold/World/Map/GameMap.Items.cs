using GoRogue.DiceNotation.Terms;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.World.Items;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

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
        entity.Position = item.Position;
        AddEntity(entity);
    }
    
    public static RogueLikeEntity CreateItem(Item item)
    {
        return
            item is Key key ? ItemFactory.Key(key.Material) :
            item is Coin ? ItemFactory.Coin() :
            throw new ArgumentException("Item not implemented.");
    }
}