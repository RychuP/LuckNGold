using Timer = SadConsole.Components.Timer;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Clock that keeps turning during enemy turns.
/// </summary>
internal class EnemyTurnClock : ScreenSurface
{
    readonly ColoredGlyphBase[] _frames = new ColoredGlyphBase[8];
    readonly Timer _timer = new(TimeSpan.FromMilliseconds(250));
    int _currentFrame = 0;

    public bool IsRunning =>
        _timer.IsRunning;

    public EnemyTurnClock() : base(1, 1)
    {
        Font = Program.Font;

        for (int i = 0; i < _frames.Length; i++)
        {
            var glyphDef = Font.GetGlyphDefinition($"Clock{i + 1}");
            _frames[i] = glyphDef.CreateColoredGlyph();
        }

        _timer.TimerElapsed += Timer_OnTimerElapsed;
        SadComponents.Add(_timer);

        AnimateHandle();
    }

    public void Start()
    {
        _timer.Start();

        // Make sure the handle turns at least once.
        AnimateHandle();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    void Timer_OnTimerElapsed(object? o, EventArgs e)
    {
        AnimateHandle();
    }

    void AnimateHandle()
    {
        _frames[_currentFrame++].CopyAppearanceTo(Surface[0]);
        Surface.IsDirty = true;

        if (_currentFrame >= _frames.Length)
            _currentFrame = 0;
    }
}