using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

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

    public void SetFontSize(int multiplier)
    {
        FontSize = Font.GetFontSize(IFont.Sizes.One) * multiplier;
        Position += Direction.Up; // Force update
        Position += Direction.Down;

        foreach (var child in Children)
            (child as IOnionLayer)!.FontSize = FontSize;
    }

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

    public IOnionLayer WeaponFar => this;
    public IOnionLayer ShieldFar => GetLayer(OnionLayerName.ShieldFar);
    public IOnionLayer Base => GetLayer(OnionLayerName.Base);
    public IOnionLayer Bodywear => GetLayer(OnionLayerName.Bodywear);
    public IOnionLayer Footwear => GetLayer(OnionLayerName.Footwear);
    public IOnionLayer Beard => GetLayer(OnionLayerName.Beard);
    public IOnionLayer Headwear => GetLayer(OnionLayerName.Headwear);
    public IOnionLayer WeaponNear => GetLayer(OnionLayerName.WeaponNear);
    public IOnionLayer RightHand => GetLayer(OnionLayerName.RightHand);
    public IOnionLayer LeftHand => GetLayer(OnionLayerName.LeftHand);

}