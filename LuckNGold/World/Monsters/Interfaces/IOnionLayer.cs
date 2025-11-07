using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It can be added as a layer to <see cref="ILayerStack"/>.
/// </summary>
internal interface IOnionLayer : IScreenSurface
{
    OnionLayerName Name { get; }
    void SetGlyph(int glyph);
}