using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives.Interfaces;

namespace LuckNGold.World.Monsters.Primitives;

/// <summary>
/// Layer that can be added to an <see cref="ILayerStack"/>.
/// </summary>
internal class OnionLayer : ScreenSurface, IOnionLayer
{
    /// <summary>
    /// Name of the layer.
    /// </summary>
    public OnionLayerName Name { get; init; }

    /// <summary>
    /// Initializes an instance of <see cref="OnionLayer"/> class.
    /// </summary>
    /// <param name="name"></param>
    public OnionLayer(OnionLayerName name) : base(1, 1)
    {
        Name = name;
    }

    public void SetGlyph(int glyph)
    {
        Surface.SetGlyph(0, 0, glyph);
    }
}