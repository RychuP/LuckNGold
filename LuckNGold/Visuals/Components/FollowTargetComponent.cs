using SadConsole;
using SadConsole.Components;

namespace LuckNGold.Visuals.Components;

/// <summary>
/// Slightly modified version of Thraka's <see cref="SurfaceComponentFollowTarget"/>.
/// </summary>
/// <param name="target"></param>
internal class FollowTargetComponent(IScreenObject target) : UpdateComponent
{
    public event EventHandler<ValueChangedEventArgs<Rectangle>>? ViewChanged;

    Point _targetPosition = Point.None;
    Rectangle _view = Rectangle.Empty;
    IScreenSurface? _host;

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
        if (_host != null)
            throw new InvalidOperationException("Component can only be added to one host.");
        if (host is not IScreenSurface screenSurface)
            throw new ArgumentException("Component can only be added to an IScreenSurface.");
        _host = screenSurface;
    }

    public override void OnRemoved(IScreenObject host)
    {
        base.OnRemoved(host);
        _host = null;
    }

    public override void Update(IScreenObject host, TimeSpan delta)
    {
        if (_targetPosition != _target.Position || _view != _host.Surface.View)
        {
            CenterViewOnTarget();
        }
    }

    public void CenterViewOnTarget()
    {
        if (_host is null) return;

        // Update cached target position.
        _targetPosition = _target.Position;

        // Calculate new rectangle for the view. 
        _host.Surface.View = _host.Surface.View.WithCenter(_targetPosition);
        if (_host.Surface.View != _view)
        {
            var prevView = _view;

            // Update cached view.
            _view = _host.Surface.View;

            OnViewChanged(prevView, _view);
        }
    }

    void OnTargetChanged(IScreenObject prevTarget, IScreenObject newTarget)
    {
        _targetPosition = newTarget.Position;
    }

    void OnViewChanged(Rectangle prevView, Rectangle newView)
    {
        var args = new ValueChangedEventArgs<Rectangle>(prevView, newView);
        ViewChanged?.Invoke(this, args);
    }
}