using LuckNGold.Generation.Decor;
using LuckNGold.Generation.Map;
using LuckNGold.World.Decor;

namespace LuckNGold.World.Map;

partial class GameMap
{
    public void PlaceDecor(IReadOnlyList<Entity> decor)
    {
        foreach (var entity in decor)
        {
            if (entity.Position == Point.None)
                throw new InvalidOperationException("All entities in the list should have " +
                    "a valid position.");

            if (entity is Steps steps)
                Place(steps);
        }
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