using LuckNGold.Visuals;
using SadRogue.Integration;
using SadRogue.Integration.FieldOfView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckNGold.Components
{
    internal class FOVHandler() : FieldOfViewHandlerBase(State.Enabled)
    {
        /// <summary>
        /// Decorator being used to tint terrain that is outside of FOV but has been explored.
        /// </summary>
        CellDecorator ExploredDecorator = new(Colors.Tint, 1, Mirror.None);

        /// <summary>
        /// Makes entity visible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntitySeen(RogueLikeEntity entity)
        {
            if (entity.Layer >= (int)GameMap.Layer.Monsters)
                entity.IsVisible = true;
        }

        /// <summary>
        /// Makes entity invisible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntityUnseen(RogueLikeEntity entity)
        {
            if (entity.Layer >= (int)GameMap.Layer.Monsters)
                entity.IsVisible = false;
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
            // Parent can't be null because of invariants enforced by structure for when
            // this function is called
            var parent = Parent!;

            // If the unseen terrain is outside of FOV, apply the decorator to tint the square appropriately.
            if (parent.PlayerExplored[terrain.Position])
                CellDecoratorHelpers.AddDecorator(ExploredDecorator, terrain.Appearance);
            else // If the unseen tile isn't explored, it's invisible
                terrain.Appearance.IsVisible = false;
        }
    }
}