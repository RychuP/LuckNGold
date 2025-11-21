using LuckNGold.World.Common.Enums;
using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

/// <summary>
/// Component for entities that need to be unlocked before access is granted.
/// </summary>
internal class LockComponent : RogueLikeComponentBase<RogueLikeEntity>, ILockable
{
    readonly CellDecorator _lockDecorator;

    /// <inheritdoc/>
    public Difficulty Difficulty { get; }

    public LockComponent(Difficulty difficulty) : base(false, false, false, false)
    {
        Difficulty = difficulty;

        // Create a decorator that will be added to the appearance of the host
        var glyphDef = Program.Font.GetGlyphDefinition($"{Difficulty}Lock");
        _lockDecorator = new CellDecorator(Color.White, glyphDef.Glyph, glyphDef.Mirror);
    }

    /// <inheritdoc/>
    public bool Unlock(IUnlocker unlocker)
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        // Anything static and big (ie door, chest) must be present on the map to be unlocked.
        if (Parent.Layer <= (int)GameMap.Layer.Furniture && Parent.CurrentMap is null)
            throw new InvalidOperationException("Furniture and below needs to be on the map.");

        if (Difficulty == (Difficulty)unlocker.Quality)
        {
            CellDecoratorHelpers.RemoveDecorator(_lockDecorator, 
                Parent.AppearanceSingle!.Appearance);
            Parent.AllComponents.Remove(this);
            return true;
        }
        return false;
    }

    public override void OnAdded(IScreenObject host)
    {
        if (host is RogueLikeEntity entity)
            CellDecoratorHelpers.AddDecorator(_lockDecorator, 
                entity.AppearanceSingle!.Appearance);
    }
}