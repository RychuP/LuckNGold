using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
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
        if (IsLocked && key.KeyColor == KeyColor)
        {
            IsLocked = false;
            return true;
        }
        return false;
    }

    public override void OnAdded(IScreenObject host)
    {
        if (host is RogueLikeEntity entity)
            CellDecoratorHelpers.AddDecorator(_lockDecorator, entity.AppearanceSingle!.Appearance);
    }

    public override void OnRemoved(IScreenObject host)
    {
        if (host is RogueLikeEntity entity)
            CellDecoratorHelpers.RemoveDecorator(_lockDecorator, entity.AppearanceSingle!.Appearance);
    }
}