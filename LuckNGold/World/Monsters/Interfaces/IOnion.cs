using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It has layers that together form a monster appearance.
/// </summary>
internal interface IOnion
{
    event EventHandler<ValueChangedEventArgs<ILayerStack>>? CurrentFrameChanged;

    /// <summary>
    /// Twelve <see cref="LayerStack"/>s that cover all frames of monster appearance.
    /// </summary>
    LayerStack[] Frames { get; }

    /// <summary>
    /// Current <see cref="ILayerStack"/> to be displayed as monster appearance.
    /// </summary>
    ILayerStack CurrentFrame { get; }

    /// <summary>
    /// Currently applied to each frame font size multiplier.
    /// </summary>
    int FontSizeMultiplier { get; }

    /// <summary>
    /// Multiplies each <see cref="LayerStack"/> font size by the given multiplier.
    /// </summary>
    /// <param name="fontSizeMultiplier">Font size multiplier that will be applied.</param>
    void SetFontSize(int fontSizeMultiplier);

    /// <summary>
    /// Selects the next frame that corresponds to the movement direction.
    /// </summary>
    /// <param name="direction">Direction of the monster entity movement.</param>
    void UpdateCurrentFrame(Direction direction);
}