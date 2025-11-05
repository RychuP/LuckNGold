using LuckNGold.World.Monsters.Interfaces;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that animate their appearance to show movement 
/// as they change their position.
/// </summary>
internal class MotionComponent : RogueLikeComponentBase<RogueLikeEntity>, IMotion
{
    /// <summary>
    /// 12 frames (3 per direction) that contain motion appearances (as per chibi samples).
    /// </summary>
    public ColoredGlyph[] Appearances { get; } = new ColoredGlyph[12];

    Direction _currentDirection = Direction.None;
    int _currentFrame = 0;
    int _frameChangeDelta = 1;

    public MotionComponent(ColoredGlyph[] appearances) : base(false, false, false, false)
    {
        CreateFrames();
        Array.Copy(appearances, Appearances, 12);
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

    void UpdateWeaponLayer()
    {

    }

    void UpdateBaseLayer(IRace race, IAppearance appearance, IEquipment equipment)
    {
        

        
    }

    void CreateFrames()
    {
        for (int i = 0; i < Appearances.Length; i++)
        {
            var frame = new ColoredGlyph()
            {
                // There are 9 layers (8 of which are decorators):
                // 0: Weapon Far
                // 1: Shield Far
                // 2: Base
                // 3: Clothes / Armour
                // 4: Beard
                // 5: Hair / Helmet
                // 6: Weapon Near
                // 7: Weapon Hand / Right Hand
                // 8: Shield Near / Left Hand
                Decorators = new(8)
            };

            Appearances[i] = frame;
        }
    }

    void AddEventHandlers()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        Parent.PositionChanged += ScreenObject_OnPositionChanged;

        var identityComponent = Parent.AllComponents.GetFirstOrDefault<IdentityComponent>() ??
            throw new InvalidOperationException("Entity needs to have an IdentityComponent.");
        identityComponent.AppearanceChanged += IIdentity_OnAppearanceChanged;

        var equipmentComponent = Parent.AllComponents.GetFirstOrDefault<EquipmentComponent>() ??
            throw new InvalidOperationException("Entity needs to have an EquipmentComponent.");
        equipmentComponent.EquipmentChanged += IEquipment_OnEquipmentChanged;
    }

    void RemoveEventHandlers()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        Parent.PositionChanged -= ScreenObject_OnPositionChanged;

        var identityComponent = Parent.AllComponents.GetFirst<IdentityComponent>();
        identityComponent.AppearanceChanged -= IIdentity_OnAppearanceChanged;

        var equipmentComponent = Parent.AllComponents.GetFirst<EquipmentComponent>();
        equipmentComponent.EquipmentChanged -= IEquipment_OnEquipmentChanged;
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);
        AddEventHandlers();
    }

    public override void OnRemoved(IScreenObject host)
    {
        RemoveEventHandlers();
        base.OnRemoved(host);
    }

    void ScreenObject_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        UpdateAppearance(Direction.GetDirection(e.OldValue, e.NewValue));
    }

    void IIdentity_OnAppearanceChanged(object? o, ValueChangedEventArgs<IAppearance> e)
    {
        
    }

    void IEquipment_OnEquipmentChanged(object? o, ValueChangedEventArgs<RogueLikeEntity?> e)
    {

    }
}