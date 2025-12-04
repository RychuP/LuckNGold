using LuckNGold.World.Monsters.Primitives;
using LuckNGold.World.Monsters.Primitives.Interfaces;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It has layers that together form a monster appearance.
/// </summary>
internal interface IOnion
{
    event EventHandler<ValueChangedEventArgs<ILayerStack>>? CurrentFrameChanged;
    event EventHandler? IsBumpingChanged;

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

    /// <summary>
    /// Sets current frame to match the facing direction.
    /// </summary>
    /// <param name="direction"></param>
    void FaceDirection(Direction direction);

    /// <summary>
    /// Sets position to all frames.
    /// </summary>
    /// <param name="position"></param>
    void SetPositions(Point position);

    /// <summary>
    /// Moves current frame a few pixels back and forth to visually show the bump action.
    /// </summary>
    /// <param name="pixelCount">Number of pixels to move current frame back and forth.</param>
    /// <param name="direction">Direction of the bump.</param>
    void Bump(int pixelCount, Direction direction);

    /// <summary>
    /// Checks if the bump animation is currently being played.
    /// </summary>
    /// <returns>True if the bump animation is being played, false otherwise.</returns>
    bool IsBumping { get; }

    /// <summary>
    /// Frame start position in pixels while bumping.
    /// </summary>
    Point BumpPosition { get; set; }
}