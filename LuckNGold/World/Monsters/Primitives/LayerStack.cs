using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;
using static SadConsole.Readers.Playscii;

namespace LuckNGold.World.Monsters.Primitives;

/// <summary>
/// Stack of layers that together form a frame of monster's motion appearance.
/// </summary>
internal class LayerStack : ScreenSurface, ILayerStack
{
    public OnionLayerName Name { get; } = OnionLayerName.WeaponFar;

    /// <summary>
    /// Initializes an instance of <see cref="LayerStack"/> class.
    /// </summary>
    public LayerStack() : base(1, 1)
    {
        var onionLayerNames = Enum.GetValues<OnionLayerName>();
        foreach (var name in onionLayerNames)
        {
            if (name == OnionLayerName.WeaponFar) continue;
            var onionLayer = new OnionLayer(name);
            Children.Add(onionLayer);
        }
    }

    /// <inheritdoc/>
    public void SetFontSize(int multiplier)
    {
        FontSize = Font.GetFontSize(IFont.Sizes.One) * multiplier;
        Position += Direction.Up; // Force update
        Position += Direction.Down;

        foreach (var child in Children)
            (child as IOnionLayer)!.FontSize = FontSize;
    }

    /// <inheritdoc/>
    public IOnionLayer GetLayer(OnionLayerName layerName)
    {
        if (layerName == OnionLayerName.WeaponFar) 
            return this;
        else
        {
            int layerIndex = (int)layerName - 1;
            return Children[layerIndex] as IOnionLayer
            ?? throw new InvalidOperationException($"Layer '{layerName}' not found in the layer stack.");
        }
    }

    public void SetGlyph(int glyph)
    {
        Surface.SetGlyph(0, 0, glyph);
    }

    /// <inheritdoc/>
    public IOnionLayer WeaponFar => this;

    /// <inheritdoc/>
    public IOnionLayer ShieldFar => GetLayer(OnionLayerName.ShieldFar);

    /// <inheritdoc/>
    public IOnionLayer Base => GetLayer(OnionLayerName.Base);

    /// <inheritdoc/>
    public IOnionLayer ClothesArmour => GetLayer(OnionLayerName.ClothesArmour);

    /// <inheritdoc/>
    public IOnionLayer Beard => GetLayer(OnionLayerName.Beard);

    /// <inheritdoc/>
    public IOnionLayer HairHelmet => GetLayer(OnionLayerName.HairHelmet);

    /// <inheritdoc/>
    public IOnionLayer WeaponNear => GetLayer(OnionLayerName.WeaponNear);

    /// <inheritdoc/>
    public IOnionLayer WeaponHandRight => GetLayer(OnionLayerName.WeaponHandRight);

    /// <inheritdoc/>
    public IOnionLayer ShieldNearLeftHand => GetLayer(OnionLayerName.ShieldNearLeftHand);
}