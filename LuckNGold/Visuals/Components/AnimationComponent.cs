using SadConsole.Components;
using SadConsole.Entities;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// A component that animates the <see cref="Entity.AppearanceSingle"/> property.<br></br><br></br>
/// This is a slightly modified version of Thraka's <see cref="AnimatedAppearanceComponent"/>, 
/// that was missing some features that I needed.
/// </summary>
public class AnimationComponent : UpdateComponent
{
    /// <summary>
    /// Occurs when the animation has finished playing (last frame was displayed).
    /// This event will not trigger if the animation is set to repeat.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the operation finishes.
    /// The event handler receives an <see cref="EventArgs"/> instance, 
    /// as no additional data is provided with the event.</remarks>
    public event EventHandler? Finished;

    int _frameIndex;
    bool _isPlaying;
    bool _isPlayingInReverse;
    TimeSpan _totalTime;
    Entity? _entity;

    /// <summary>
    /// The frames of animation.
    /// </summary>
    ColoredGlyph[] _frames = [];
    public ColoredGlyph[] Frames
    {
        get => _frames;
        set
        {
            _frames = value ?? [];

            if (_frames.Length < 2)
                throw new ArgumentException("This component requires animations with " +
                    "at least two frames.");

            _frameIndex = 0;
            if (IsReversable)
                _isPlayingInReverse = false;
        }
    }

    /// <summary>
    /// The time it takes to play one frame.
    /// </summary>
    public TimeSpan FrameLength { get; set; } = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// When <see langword="true"/>, the animation will automatically restart 
    /// after the last frame is applied. Otherwise, <see langword="false"/> 
    /// and the animation stops when completed.
    /// </summary>
    public bool IsRepeatable { get; set; }
    
    /// <summary>
    /// If this property is set to true and <see cref="IsRepeatable"/> is set to true, 
    /// Animation will alternate between playing forwards and backwards.
    /// </summary>
    public bool IsReversable { get; set; }

    /// <summary>
    /// Called by the component system when this component is added to an object. Must be of type <see cref="Entity"/>.
    /// </summary>
    /// <param name="host">The component host.</param>
    /// <exception cref="InvalidCastException">This component was added to a type other than <see cref="Entity"/>.</exception>
    public override void OnAdded(IScreenObject host)
    {
        if (host is not Entity entity) 
            throw new InvalidCastException("Component can only be used on an entity.");

        _entity = entity;
    }

    /// <summary>
    /// Called by the component system when this component is removed from an object.
    /// </summary>
    /// <param name="host">The component host.</param>
    public override void OnRemoved(IScreenObject host) =>
        _entity = null;

    /// <summary>
    /// Updates the animation frame index and applies the animation to the entity.
    /// </summary>
    /// <param name="host">The component host.</param>
    /// <param name="delta">The time between calls to this method.</param>
    public override void Update(IScreenObject host, TimeSpan delta)
    {
        if (!_isPlaying) return;

        if (_entity!.IsSingleCell)
        {
            _totalTime += delta;

            if (_totalTime >= FrameLength)
            {
                _totalTime -= FrameLength;

                if (!_isPlayingInReverse)
                {
                    _frameIndex++;

                    if (_frameIndex >= _frames.Length)
                    {
                        _frameIndex = _frames.Length - 1;

                        if (IsRepeatable)
                        {
                            if (IsReversable)
                            {
                                _isPlayingInReverse = true;

                                // We know from the Frames property checks, that there are
                                // at least two frames in this animation.
                                _frameIndex--;
                            }
                            else
                                _frameIndex = 0;
                        }
                        else
                        {
                            OnFinished();
                            return;
                        }
                    }
                }
                else
                {
                    _frameIndex--;
                    
                    if (_frameIndex <= -1)
                    {
                        // We know from the Frames property checks, that there are
                        // at least two frames in this animation.
                        _frameIndex = 1;

                        // We know that IsRepeatable is true, as otherwise
                        // code would not reach this section, hence no check.
                        _isPlayingInReverse = false;
                    }
                }
                _frames[_frameIndex].CopyAppearanceTo(_entity.AppearanceSingle.Appearance, false);
            }
        }
    }

    /// <summary>
    /// Starts the animation and immediately applies the current frame to the entity.
    /// </summary>
    /// <exception cref="InvalidOperationException">The animation was started 
    /// but there aren't any frames to animate.</exception>
    public void Start()
    {
        if (_frames.Length == 0) 
            throw new InvalidOperationException("Animation was started " +
                "but there aren't any frames to animate");

        if (_entity == null) 
            throw new Exception("Component must be added to an entity.");

        if (_entity.IsSingleCell)
        {
            _isPlaying = true;
            _frames[_frameIndex].CopyAppearanceTo(_entity.AppearanceSingle.Appearance, false);
        }
    }

    /// <summary>
    /// Stops the animation.
    /// </summary>
    public void Stop()
    {
        _isPlaying = false;
    }

    /// <summary>
    /// Restarts the animation at the first frame.
    /// </summary>
    public void Restart()
    {
        _frameIndex = 0;
        if (IsReversable)
            _isPlayingInReverse = false;
        Start();
    }

    void OnFinished()
    {
        Stop();
        Finished?.Invoke(this, EventArgs.Empty);
    }
}