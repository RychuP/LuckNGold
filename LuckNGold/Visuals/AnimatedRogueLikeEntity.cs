using LuckNGold.Visuals.Components;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.Visuals;

/// <summary>
/// Base entity that can play various, named animations depending on the state.
/// </summary>
/// <remarks>Animations can have multiple or a single frame. Both cases are handled 
/// by the <see cref="AnimationComponent"/>.</remarks>
internal class AnimatedRogueLikeEntity : RogueLikeEntity
{
    /// <summary>
    /// Fired when <see cref="CurrentAnimation"/> has changed.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs<string>>? AnimationChanged;

    /// <summary>
    /// Fired when <see cref="CurrentAnimation"/> finished playing.
    /// </summary>
    public event EventHandler? Finished;

    string _defaultAnimation = string.Empty;
    /// <summary>
    /// Default animation that can be played when idle.
    /// </summary>
    public string DefaultAnimation
    {
        get => _defaultAnimation;
        set
        {
            if (_defaultAnimation == value) return;

            if (!HasAnimation(value))
                throw new InvalidOperationException("Default animation is not in the animations.");
            
            _defaultAnimation = value;
        }
    }

    // List of all animations available to be played
    readonly Dictionary<string, ColoredGlyph[]> _animations;

    // Component that plays the animations
    readonly AnimationComponent _animationComponent;

    /// <summary>
    /// Default animation will play forwards and than backwards if true.
    /// </summary>
    public bool DefaultAnimationIsReversable { get; set; }

    /// <summary>
    /// Whether the default animation should automatically start playing when
    /// current animation is finished.
    /// </summary>
    public bool PlaysDefaultAnimationOnFinished { get; set; } = false;

    /// <summary>
    /// True whe <see cref="AnimationComponent"/> is playing frames
    /// and false when only one frame is set as appearance.
    /// </summary>
    public bool IsPlaying => _animationComponent.IsPlaying;

    /// <summary>
    /// Appearance of the entity for the purpose of displaying it in various information windows.
    /// </summary>
    public ColoredGlyphBase StaticAppearance =>
        _animations[DefaultAnimation][0];

    string _currentAnimation = string.Empty;
    /// <summary>
    /// Name of the animation currently being played.
    /// </summary>
    public string CurrentAnimation
    {
        get => _currentAnimation;
        set
        {
            if (!_animations.ContainsKey(value))
                throw new ArgumentException("There is no animation with the given name.", 
                    nameof(value));

            string prevAnim = _currentAnimation;
            _currentAnimation = value;

            if (prevAnim != _currentAnimation)
                // start animation and trigger event
                OnCurrentAnimationChanged(prevAnim, _currentAnimation);
            else
                // quietly restart the current animation
                StartCurrentAnimation();
        }
    }

    /// <summary>
    /// Entity that has an animatied appearance. Several animations can be defined.<br></br>
    /// This constructor creates an instance with a single animation.
    /// </summary>
    public AnimatedRogueLikeEntity(string defaultAnimation, bool defaultAnimIsReversable,
        GameMap.Layer layer, bool walkable = true, bool transparent = true)
        : this([defaultAnimation], defaultAnimation, defaultAnimIsReversable, 
              layer, walkable, transparent)
    { }

    /// <summary>
    /// Entity that has an animatied appearance. Several animations can be defined.<br></br>
    /// This constructor creates an instance with a single animation.
    /// </summary>
    public AnimatedRogueLikeEntity(Point position, string defaultAnimation,
        bool defaultAnimIsReversable, GameMap.Layer layer, 
        bool walkable = true, bool transparent = true)
        : this([defaultAnimation], defaultAnimation, defaultAnimIsReversable,
              layer, walkable, transparent)
    {
        Position = position;
    }

    /// <summary>
    /// Entity that has an animatied appearance. Several animations can be defined.
    /// </summary>
    public AnimatedRogueLikeEntity(Point position, string[] animations, string defaultAnimation,
        bool defaultAnimIsReversable, GameMap.Layer layer, 
        bool walkable = true, bool transparent = true)
        : this(animations, defaultAnimation, defaultAnimIsReversable, layer, walkable, transparent)
    {
        Position = position;
    }

    /// <summary>
    /// Entity that has an animatied appearance. Several animations can be defined.
    /// </summary>
    /// <param name="animations">Collection of names for animations to be extracted 
    /// from the Font for the entity.</param>
    /// <param name="defaultAnimation">The name of the default animation to be used for the entity.</param>
    /// <param name="layer">The layer of the game map where the entity resides.</param>
    /// <param name="walkable">A value indicating whether the entity can be walked over.
    /// The default value is <see langword="true"/>.</param>
    /// <param name="transparent">A value indicating whether the entity is transparent, 
    /// allowing visibility through it. The default value is <see langword="true"/>.</param>
    public AnimatedRogueLikeEntity(string[] animations, string defaultAnimation,
        bool defaultAnimIsReversable, GameMap.Layer layer, bool walkable = true, bool transparent = true) 
        : base(new ColoredGlyph(Color.White, Color.Transparent, 2), 
            walkable, transparent, (int) layer)
    {
        // Get all the animations for future reference
        _animations = [];
        foreach (var animation in animations)
        {
            var frames = GetFrames(animation);
            _animations[animation] = frames;
        }

        DefaultAnimation = defaultAnimation;
        DefaultAnimationIsReversable = defaultAnimIsReversable;

        // Create the component to play the animations
        _animationComponent = new AnimationComponent();
        _animationComponent.Finished += AnimatedComponent_OnFinished;
        AllComponents.Add(_animationComponent);

        // Start animating
        PlayDefaultAnimation();
    }

    public bool HasAnimation(string animation) =>
        _animations.ContainsKey(animation);

    /// <summary>
    /// Pauses animating the entity without changing current animation or frame played.
    /// </summary>
    public void PauseAnimating()
    {
        _animationComponent.Stop();
    }

    /// <summary>
    /// Resumes animating the entity from the frame where the pause was called.
    /// </summary>
    public void ResumeAnimating()
    {
        _animationComponent.Start();
    }

    /// <summary>
    /// Reverts to default animation and stops animating the entity.
    /// </summary>
    public void StopAnimating()
    {
        PlayDefaultAnimation();
        _animationComponent.Stop();
    }

    /// <summary>
    /// Starts animation from the first frame.
    /// </summary>
    public void StartAnimating()
    {
        _animationComponent.Restart();
    }

    /// <summary>
    /// Plays given animation.
    /// </summary>
    /// <param name="animation">Animation to be played.</param>
    /// <param name="isRepeatable">Whether the animation should play indefinitely.</param>
    public void PlayAnimation(string animation, bool isRepeatable = false, bool isReversable = false)
    {
        CurrentAnimation = animation;
        _animationComponent.IsRepeatable = isRepeatable;
        _animationComponent.IsReversable = isReversable;
    }

    public void PlayDefaultAnimation() =>
        PlayAnimation(DefaultAnimation, true, DefaultAnimationIsReversable);

    /// <summary>
    /// Retrieves an array of frames for the specified animation.
    /// </summary>
    /// <remarks>Each frame in the returned array is initialized with default foreground and background
    /// colors, and the glyph and mirror values are populated based on the animation's font definitions.</remarks>
    /// <param name="animationName">The name of the animation for which to retrieve frames.</param>
    /// <returns>An array of <see cref="ColoredGlyphAndEffect"/> objects representing the frames of the animation. The array will
    /// be empty if no frames are defined for the specified animation.</returns>
    static ColoredGlyphAndEffect[] GetFrames(string animationName)
    {
        var definitions = GetFontDefinitions(animationName);
        if (definitions.Length == 0) 
            throw new ArgumentException("Animation with the given name does not exist.",
                nameof(animationName));

        var frames = new ColoredGlyphAndEffect[definitions.Length];

        for (int i = 0; i < definitions.Length; i++)
        {
            var definition = definitions[i];
            var frame = new ColoredGlyphAndEffect
            {
                Foreground = Color.White,
                Background = Color.Transparent,
                Glyph = definition.Glyph,
                Mirror = definition.Mirror
            };
            frames[i] = frame;
        }
        return frames;
    }

    /// <summary>
    /// Retrieves an array of glyph definitions for fonts whose names start 
    /// with the specified string.
    /// </summary>
    /// <param name="name">The prefix to match against font names. 
    /// Only glyph definitions for fonts with names starting with this value
    /// will be included.</param>
    /// <returns>An array of <see cref="GlyphDefinition"/> objects representing 
    /// the glyphs of matching fonts. The array will be
    /// empty if no matching fonts are found.</returns>
    static GlyphDefinition[] GetFontDefinitions(string name)
    {
        return [.. Program.Font.GlyphDefinitions
            .Where(g => g.Key.StartsWith(name))
            .OrderBy(g => g.Key)
            .Select(g => g.Value)];
    }

    // Resets animation component to start the current animation.
    void StartCurrentAnimation()
    {
        _animationComponent.Stop();
        var frames = _animations[_currentAnimation];

        if (frames.Length == 1)
        {
            frames[0].CopyAppearanceTo(AppearanceSingle!.Appearance);
        }
        else
        {
            _animationComponent.Frames = frames;
            _animationComponent.Start();
        }
    }

    // Starts current animation and triggers the AnimationChanged event.
    void OnCurrentAnimationChanged(string prevAnimation, string newAnimation)
    {
        StartCurrentAnimation();

        var args = new ValueChangedEventArgs<string>(prevAnimation, newAnimation);
        AnimationChanged?.Invoke(this, args);
    }

    void AnimatedComponent_OnFinished(object? o, EventArgs e)
    {
        Finished?.Invoke(this, e);

        if (PlaysDefaultAnimationOnFinished)
            PlayDefaultAnimation();
    }
}