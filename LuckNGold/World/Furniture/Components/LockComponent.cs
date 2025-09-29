using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using SadConsole.Entities;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furniture.Components;

/// <summary>
/// Component for entities that can be locked/unlocked.
/// </summary>
internal class LockComponent : RogueLikeComponentBase<RogueLikeEntity>, ILockable
{
    readonly CellDecorator _lockDecorator;

    /// <inheritdoc/>
    public KeyColor KeyColor { get; }

    /// <inheritdoc/>
    public bool IsLocked { get; private set; } = true;

    public LockComponent(KeyColor keyColor) : base(false, false, false, false)
    {
        KeyColor = keyColor;

        // Create decorator that will be added to the appearance of the host
        var glyphDef = Program.Font.GetGlyphDefinition($"{KeyColor}Lock");
        _lockDecorator = new CellDecorator(Color.White, glyphDef.Glyph, glyphDef.Mirror);
    }

    /// <inheritdoc>/>
    public bool Unlock(IKey key)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (IsLocked && key.KeyColor == KeyColor)
        {
            IsLocked = false;
            CellDecoratorHelpers.RemoveDecorator(_lockDecorator, Parent.AppearanceSingle!.Appearance);
            Parent.AllComponents.Remove(this);
            return true;
        }
        return false;
    }

    public override void OnAdded(IScreenObject host)
    {
        if (host is RogueLikeEntity entity)
            CellDecoratorHelpers.AddDecorator(_lockDecorator, entity.AppearanceSingle!.Appearance);
    }
}