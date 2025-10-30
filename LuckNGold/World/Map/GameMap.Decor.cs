using LuckNGold.Generation.Decors;
using LuckNGold.World.Decors;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates decor data objects from generator to RogueLike entities.
partial class GameMap
{
    public void PlaceDecor(Decor decor)
    {
        if (decor.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(decor.Position);
        if (entity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        entity = CreateDecor(decor);
        AddEntity(entity, decor.Position);
    }

    /// <summary>
    /// Converts data object into a <see cref="RogueLikeEntity"/> that can be placed on the map.
    /// </summary>
    /// <param name="decor">Data object from generator.</param>
    /// <returns>An instance of <see cref="RogueLikeEntity"/> 
    /// created from given data object.</returns>
    /// <exception cref="ArgumentException"></exception>
    static RogueLikeEntity CreateDecor(Decor decor) =>
        decor is Steps steps ? DecorFactory.Steps(steps.FaceRight, steps.LeadDown) :
        decor is Banner banner ? DecorFactory.Banner(banner.Gemstone) :
        decor is SideTorch sideTorch ? DecorFactory.SideTorch(sideTorch.Orientation) :
        decor is Torch ? DecorFactory.Torch() :
        decor is Fountain fountain ? DecorFactory.Fountain(fountain.Orientation, fountain.IsBlue) :
        decor is Shackle shackle ? DecorFactory.Shackle(shackle.Size) :
        decor is Boxes boxes ? DecorFactory.Boxes(boxes.Size) :
        decor is SpiderWeb spiderWeb ? DecorFactory.SpiderWeb(spiderWeb.Size, spiderWeb.Orientation) :
        decor is CandleStand candleStand ? DecorFactory.CandleStand(candleStand.Size) :
        decor is Candle candle ? DecorFactory.Candle(candle.Size) :
        decor is Skull skull ? DecorFactory.Skull(skull.Orientation) :
        decor is Bones ? DecorFactory.Bones() :
        decor is Urn ? DecorFactory.Urn() :
        decor is Cauldron ? DecorFactory.Cauldron() :
        throw new ArgumentException("Item not implemented.");
}