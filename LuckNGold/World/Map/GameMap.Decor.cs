using LuckNGold.Generation.Decors;
using LuckNGold.World.Decors;
using LuckNGold.World.Items.Enums;
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
        decor is Flag flag ? DecorFactory.Flag(flag.Gemstone) :
        decor is SideTorch sideTorch ? DecorFactory.SideTorch(sideTorch.Orientation) :
        decor is Torch ? DecorFactory.Torch() :
        decor is FountainTop fountainTop ? DecorFactory.FountainTop(fountainTop.Color) :
        decor is FountainBottom fountainBottom ? DecorFactory.FountainBottom(fountainBottom.Color) :
        decor is Shackle shackle ? DecorFactory.Shackle(shackle.Size) :
        throw new ArgumentException("Item not implemented.");

    /// <summary>
    /// Places steps to the upper and lower level.
    /// </summary>
    void Place(Steps stepsData)
    {
        var steps = DecorFactory.Steps(stepsData.FaceRight, stepsData.LeadDown);
        AddEntity(steps, stepsData.Position);
    }
}