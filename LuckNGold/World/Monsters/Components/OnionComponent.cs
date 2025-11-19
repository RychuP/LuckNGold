using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that combine their appearance from multiple layers (like an onion).
/// </summary>
partial class OnionComponent : RogueLikeComponentBase<RogueLikeEntity>, IOnion
{
    public event EventHandler<ValueChangedEventArgs<ILayerStack>>? CurrentFrameChanged;

    /// <summary>
    /// Parent's current direction of movement.
    /// </summary>
    Direction _currentDirection = Direction.None;
    int _currentMotionStep = 0;
    int _frameChangeDelta = 1;

    /// <summary>
    /// Parent's identity component.
    /// </summary>
    IdentityComponent IdentityComponent => 
        Parent!.AllComponents.GetFirst<IdentityComponent>();

    /// <summary>
    /// Parent's equipment component.
    /// </summary>
    EquipmentComponent EquipmentComponent =>
        Parent!.AllComponents.GetFirst<EquipmentComponent>();

    /// <summary>
    /// Array of motion frames (3 per direction).
    /// </summary>
    public LayerStack[] Frames { get; } = new LayerStack[12];

    /// <summary>
    /// Currently applied font size multiplier.
    /// </summary>
    public int FontSizeMultiplier { get; private set; } = -1;

    ILayerStack _currentFrame;
    public ILayerStack CurrentFrame
    {
        get => _currentFrame;
        set
        {
            if (_currentFrame == value) return;
            var prevFrame = _currentFrame;
            _currentFrame = value;
            OnCurrentFrameChanged(prevFrame, value);
        }
    }
    
    /// <summary>
    /// Initializes an instance of <see cref="OnionComponent"/> class.
    /// </summary>
    public OnionComponent() : base(false, false, false, false)
    {
        // Create layer stacks.
        for (int i = 0; i < Frames.Length; i++)
            Frames[i] = new LayerStack();

        _currentFrame = Frames[1];
    }

    public void SetFontSize(int fontSizeMultiplier)
    {
        foreach (var frame in Frames)
            frame.SetFontSize(fontSizeMultiplier);
        FontSizeMultiplier = fontSizeMultiplier;
    }

    public void UpdateCurrentFrame(Direction direction)
    {
        direction =
            direction.DeltaX < 0 ? Direction.Left :
            direction.DeltaX > 0 ? Direction.Right :
            direction.DeltaY < 0 ? Direction.Up :
            direction.DeltaY > 0 ? Direction.Down :
            Direction.None;

        if (_currentDirection != direction)
        {
            _currentDirection = direction;
            _currentMotionStep = 1;
            _frameChangeDelta = 1;
        }
        else
        {
            _currentMotionStep = _currentMotionStep + _frameChangeDelta;
            if (_currentMotionStep > 2)
            {
                _frameChangeDelta = -1;
                _currentMotionStep = 1;
            }
            else if (_currentMotionStep < 0)
            {
                _frameChangeDelta = 1;
                _currentMotionStep = 1;
            }
        }

        // Initial index in the row of motion appearances.
        int motionIndex = _currentDirection.Type switch
        {
            Direction.Types.Left => 3,
            Direction.Types.Right => 6,
            Direction.Types.Up => 9,
            _ => 0,
        };

        int frameIndex = motionIndex + _currentMotionStep;
        CurrentFrame = Frames[frameIndex];
    }


    /// <summary>
    /// Sets all frames on the given layer to the font provided and glyphs that
    /// represent all frames of entity motion on that layer.
    /// </summary>
    /// <param name="layerName">Name of the <see cref="IOnionLayer"/> to be set.</param>
    /// <param name="fontName">Font name to be applied to the layer.</param>
    /// <param name="row">Row of the first glyph.</param>
    /// <param name="column">Column of the first glyph.</param>
    void SetLayerAppearance(OnionLayerName layerName, string fontName, int row, int column)
    {
        if (!Game.Instance.Fonts.TryGetValue(fontName, out IFont? font)) return;
        if (font is not SadFont sadFont) return;

        // All chibi font textures have been moved down by that amount of rows.
        int rowOffset = 1;

        // Initial glyph from which iteration starts.
        int firstGlyphIndex = (row + rowOffset) * sadFont.Columns + column;

        // Set appropriate motion glyph on the given layer of each frame.
        for (int i = 0, y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++, i++)
            {
                int deltaY = sadFont.Columns * y;
                int glyph = firstGlyphIndex + deltaY + x;
                var layer = Frames[i].GetLayer(layerName);
                layer.Font = font;
                layer.FontSize = font.GetFontSize(IFont.Sizes.One) * FontSizeMultiplier;
                layer.SetGlyph(glyph);
            }
        }
    }

    /// <summary>
    /// Sets the glyph to 0 on the given layer.
    /// </summary>
    /// <param name="layerName">Layer to be erased.</param>
    void EraseLayer(OnionLayerName layerName)
    {
        foreach (var frame in Frames)
        {
            var layer = frame.GetLayer(layerName);
            layer.SetGlyph(0);
        }
    }

    /// <summary>
    /// Draws all frames when the component is first added to an entity,
    /// </summary>
    void DrawInitialAppearance()
    {
        var equipment = EquipmentComponent;

        UpdateBaseLayer();
        UpdateBodywearLayer();
        UpdateFootwearLayer();
        UpdateBeard();
        UpdateHeadwearLayer();

        if (equipment.RightHand is RogueLikeEntity weapon)
            DrawWeapon(weapon);
        else
            DrawRightEmptyHand();

        if (equipment.LeftHand is RogueLikeEntity shield)
            DrawShield(shield);
        else
            DrawLeftEmptyHand();
    }

    void DrawWeapon(RogueLikeEntity weapon)
    {
        var materialComponent = weapon.AllComponents.GetFirst<IMaterial>();
        var material = materialComponent.Material;
        string fontName = "weapons-1";
        int row = 0, col = 0;

        if (weapon.AllComponents.Contains<IMeleeAttack>())
        {
            if (weapon.Name.Contains("Sword"))
            {
                row = weapon.Name.Contains("Arming") ? 0 :
                    weapon.Name.Contains("Gladius") ? 0 :
                    weapon.Name.Contains("Scimitar") ? 0 :
                    throw new InvalidOperationException("Unknown sword.");

                col = weapon.Name.Contains("Arming") ? 0 :
                    weapon.Name.Contains("Gladius") ? 1 :
                    weapon.Name.Contains("Scimitar") ? 2 :
                    throw new InvalidOperationException("Unknown sword.");
            }
        }

        DrawWeaponFar(fontName, row, col);
        DrawWeaponNear(fontName, row, col);
        DrawRightWeaponHand(row, col);
    }

    void EraseWeapon()
    {
        EraseWeaponFar();
        EraseWeaponNear();
        DrawRightEmptyHand();
    }

    void DrawShield(RogueLikeEntity shield)
    {

    }

    static string GetRaceTypeText(Race race) =>
        race.RaceType.ToString().ToLower();

    static string GetSkinToneText(Race race)
    {
        return race.SkinTone switch
        {
            SkinTone.Pale => "-pale",
            SkinTone.Dark => "-dark",
            _ => string.Empty,
        };
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
        IdentityComponent.AppearanceChanged -= IIdentity_OnAppearanceChanged;
        EquipmentComponent.EquipmentChanged -= IEquipment_OnEquipmentChanged;
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);
        AddEventHandlers();
        DrawInitialAppearance();
    }

    public override void OnRemoved(IScreenObject host)
    {
        RemoveEventHandlers();
        base.OnRemoved(host);
    }

    void OnCurrentFrameChanged(ILayerStack prevFram, ILayerStack newFrame)
    {
        var args = new ValueChangedEventArgs<ILayerStack>(prevFram, newFrame);
        CurrentFrameChanged?.Invoke(this, args);
    }

    void ScreenObject_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        UpdateCurrentFrame(Direction.GetDirection(e.OldValue, e.NewValue));
    }

    void IIdentity_OnAppearanceChanged(object? o, ValueChangedEventArgs<Appearance> e)
    {
        DrawInitialAppearance();
    }

    void IEquipment_OnEquipmentChanged(object? o, ValueChangedEventArgs<RogueLikeEntity?> e)
    {
        var item =
            e.NewValue is not null ? e.NewValue :
            e.OldValue is not null ? e.OldValue :
            throw new InvalidOperationException("Event with both values set to null");

        var equippable = item.AllComponents.GetFirst<IEquippable>();
        var equipSlot = equippable.Slot;
        switch (equipSlot)
        {
            case EquipSlot.RightHand:
                if (item == e.NewValue)
                    DrawWeapon(item);
                else
                    EraseWeapon();
                break;

            default:
                DrawInitialAppearance();
                break;
        }
    }
}