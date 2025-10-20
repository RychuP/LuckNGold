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
        decor is Flag flag ? GetFlag(flag.Gemstone) : // DecorFactory.Flag(flag.Gemstone) :
        throw new ArgumentException("Item not implemented.");

    static RogueLikeEntity GetFlag(Gemstone gem)
    {
        var flag = DecorFactory.Flag(gem);
        return flag;
    }

    /// <summary>
    /// Places steps to the upper and lower level.
    /// </summary>
    void Place(Steps stepsData)
    {
        var steps = DecorFactory.Steps(stepsData.FaceRight, stepsData.LeadDown);
        AddEntity(steps, stepsData.Position);
    }
}