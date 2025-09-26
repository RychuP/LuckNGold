using LuckNGold.Visuals;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView;

namespace LuckNGold.World;

/// <summary>
/// Controls visibility and tint of terrain and entities dependind on 
/// whether they have been explored and are within player's view.
/// </summary>
internal class MapFOVHandler() : FieldOfViewHandlerBase(State.Enabled)
{
    /// <summary>
    /// Decorator being used to tint terrain that is outside of FOV but has been explored.
    /// </summary>
    readonly CellDecorator ExploredDecorator = new(Colors.Tint, 1, Mirror.None);

    /// <summary>
    /// Makes entity visible.
    /// </summary>
    /// <param name="entity">Entity to modify.</param>
    protected override void UpdateEntitySeen(RogueLikeEntity entity)
    {
        entity.IsVisible = true;
        CellDecoratorHelpers.RemoveDecorator(ExploredDecorator, 
            entity.AppearanceSingle!.Appearance);
        if (entity is AnimatedRogueLikeEntity animatedEntity)
            animatedEntity.StartAnimating();
    }

    /// <summary>
    /// Makes entity invisible.
    /// </summary>
    /// <param name="entity">Entity to modify.</param>
    protected override void UpdateEntityUnseen(RogueLikeEntity entity)
    {
        // Check entity is an item or above and make it invisible.
        if (entity.Layer >= (int)GameMap.Layer.Items)
        {
            entity.IsVisible = false;

            // Stop animation if playing.
            if (entity is AnimatedRogueLikeEntity animatedEntity)
                animatedEntity.StopAnimating();
        }

        // Everything else gets tinted if explored.
        else
        {
            // Check the entity is in player's fov.
            if (Parent!.PlayerExplored[entity.Position])
            {
                // Stop animation if playing.
                if (entity is AnimatedRogueLikeEntity animatedEntity)
                    animatedEntity.StopAnimating();

                // Add tint to entity.
                CellDecoratorHelpers.AddDecorator(ExploredDecorator, 
                    entity.AppearanceSingle!.Appearance);
            }

            // If the unseen entity isn't explored, it's invisible
            else
                entity.AppearanceSingle!.Appearance.IsVisible = false;
        }
    }

    /// <summary>
    /// Makes terrain visible and sets its foreground color to its regular value.
    /// </summary>
    /// <param name="terrain">Terrain to modify.</param>
    protected override void UpdateTerrainSeen(RogueLikeCell terrain)
    {
        terrain.Appearance.IsVisible = true;
        CellDecoratorHelpers.RemoveDecorator(ExploredDecorator, terrain.Appearance);
    }

    /// <summary>
    /// Makes terrain invisible if it is not explored.  Makes terrain visible but tints it using
    /// <see cref="ExploredDecorator"/> if it is explored.
    /// </summary>
    /// <param name="terrain">Terrain to modify.</param>
    protected override void UpdateTerrainUnseen(RogueLikeCell terrain)
    {
        // If the unseen terrain is outside of FOV, apply the decorator to tint the square appropriately.
        if (Parent!.PlayerExplored[terrain.Position])
        {
            // Don't add decorators to terrain with furniture or decor on
            // as they will also get a tint which will make the tile too dark
            var isEmpty = !Parent!.Entities.Any(
                e => e.Position == terrain.Position &&
                e.Item is RogueLikeEntity re && 
                re.Layer <= (int)GameMap.Layer.Furniture);
            if (isEmpty)
                CellDecoratorHelpers.AddDecorator(ExploredDecorator, terrain.Appearance);
        }
        else // If the unseen tile isn't explored, it's invisible
            terrain.Appearance.IsVisible = false;
    }
}