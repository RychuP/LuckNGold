using LuckNGold.Generation.Decors;
using LuckNGold.World.Decors;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates generated decor entities to RogueLike entities.
partial class GameMap
{
    public void PlaceDecor(Decor decor)
    {
        if (decor.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(decor.Position);
        if (entity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        if (decor is Steps steps)
            Place(steps);
    }

    /// <summary>
    /// Places steps to the upper and lower level.
    /// </summary>
    public void Place(Steps data)
    {
        var steps = DecorFactory.Steps(data.FaceRight, data.LeadDown);
        AddEntity(steps, data.Position);
    }
}