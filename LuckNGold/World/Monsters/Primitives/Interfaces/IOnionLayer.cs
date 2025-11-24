using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Primitives.Interfaces;

/// <summary>
/// It can be added as a layer to <see cref="ILayerStack"/>.
/// </summary>
internal interface IOnionLayer : IScreenSurface
{
    /// <summary>
    /// Name of the layer.
    /// </summary>
    OnionLayerName Name { get; }

    /// <summary>
    /// Sets glyph in the only cell of the layer.
    /// </summary>
    /// <param name="glyph">Glyph index to be set.</param>
    void SetGlyph(int glyph);
}