using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that animate their appearance to show movement 
/// as they change their position.
/// </summary>
internal class MotionAppearanceComponent : 
    RogueLikeComponentBase<RogueLikeEntity>, IMotionAppearance
{
    /// <summary>
    /// 12 frames (3 per direction) that contain motion appearances (as per chibi samples).
    /// </summary>
    public ColoredGlyph[] Appearances { get; }

    Direction _currentDirection = Direction.None;
    int _currentFrame = 0;
    int _frameChangeDelta = 1;

    public MotionAppearanceComponent(ColoredGlyph[] appearances) :
        base(false, false, false, false)
    {
        if (appearances.Length != 12)
            throw new ArgumentException("There needs to be exactly 12 frames of appearances.");

        Appearances = appearances;
    }

    public void UpdateAppearance(Direction direction)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        //if (Parent.CurrentMap == null)
        //    //throw new InvalidOperationException("Parent has to be on the map.");
        //    return;

        direction =
            direction.DeltaX < 0 ? Direction.Left :
            direction.DeltaX > 0 ? Direction.Right :
            direction.DeltaY < 0 ? Direction.Up :
            direction.DeltaY > 0 ? Direction.Down :
            Direction.None;

        if (_currentDirection != direction)
        {
            _currentDirection = direction;
            _currentFrame = 1;
            _frameChangeDelta = 1;
        }
        else
        {
            _currentFrame = _currentFrame + _frameChangeDelta;
            if (_currentFrame > 2)
            {
                _frameChangeDelta = -1;
                _currentFrame = 1;
            }
            else if (_currentFrame < 0)
            {
                _frameChangeDelta = 1;
                _currentFrame = 1;
            }
        }

        int appearanceIndex = _currentDirection.Type switch
        {
            Direction.Types.Left => 3,
            Direction.Types.Right => 6,
            Direction.Types.Up => 9,
            _ => 0,
        };

        var appearance = Appearances[appearanceIndex + _currentFrame];
        appearance.CopyAppearanceTo(Parent.AppearanceSingle!.Appearance);
    }

    void ScreenObject_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        UpdateAppearance(Direction.GetDirection(e.OldValue, e.NewValue));
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);
        host.PositionChanged += ScreenObject_OnPositionChanged;
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);
        host.PositionChanged -= ScreenObject_OnPositionChanged;
    }
}