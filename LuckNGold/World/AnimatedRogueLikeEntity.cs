using SadConsole.Entities;
using SadRogue.Integration;

namespace LuckNGold.World;

internal class AnimatedRogueLikeEntity : RogueLikeEntity
{
    public const double FrameLength = 200d;

    public AnimatedRogueLikeEntity(Point position) 
        : base(position, new ColoredGlyph(Color.White, Color.Transparent, 2))
    {
        var comp = new AnimatedAppearanceComponent();
        comp.IsRepeatable = true;
        comp.Frames = GetFrames("Flag");
        comp.AnimationTime = TimeSpan.FromMilliseconds(FrameLength * comp.Frames.Length);
        AllComponents.Add(comp);
        comp.Start();
    }

    static ColoredGlyphAndEffect[] GetFrames(string animationName)
    {
        var definitions = GetFontDefinitions(animationName);
        var frames = new ColoredGlyphAndEffect[definitions.Length];
        if (frames.Length == 0) 
            return frames;

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
}