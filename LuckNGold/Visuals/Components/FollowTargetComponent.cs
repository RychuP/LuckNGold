using SadConsole.Components;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Slightly modified version of Thraka's <see cref="SurfaceComponentFollowTarget"/>.
/// </summary>
/// <param name="target"></param>
internal class FollowTargetComponent(IScreenObject target) : UpdateComponent
{
    public event EventHandler? ViewChanged;

    Point _targetPosition = Point.None;
    Rectangle _view = Rectangle.Empty;

    IScreenObject _target = target;
    /// <summary>
    /// Target to have the surface follow.
    /// </summary>
    public IScreenObject Target
    {
        get => _target;
        set
        {
            if (_target == value) return;
            var prevTarget = _target;
            _target = value;
            OnTargetChanged(prevTarget, value);
        }
    }

    /// <inheritdoc/>
    public override void OnAdded(IScreenObject host)
    {
        if (host is not IScreenSurface)
            throw new ArgumentException("Component can only be added to an IScreenSurface.");
    }

    public override void Update(IScreenObject host, TimeSpan delta)
    {
        if (host is not IScreenSurface screenSurface) return;

        if (_targetPosition != _target.Position || _view != screenSurface.Surface.View)
        {
            // Update cached target position.
            _targetPosition = _target.Position;

            // Calculate new rectangle for the view.
            screenSurface.Surface.View = screenSurface.Surface.View.WithCenter(_targetPosition);
            if (screenSurface.Surface.View != _view)
            {
                // Update cached view.
                _view = screenSurface.Surface.View;
                OnViewChanged();
            }
        }
    }

    void OnTargetChanged(IScreenObject prevTarget, IScreenObject newTarget)
    {
        _targetPosition = newTarget.Position;
    }

    void OnViewChanged()
    {
        ViewChanged?.Invoke(this, EventArgs.Empty);
    }
}