using LuckNGold.Config;
using LuckNGold.Visuals.Components;
using LuckNGold.World.Common.Interfaces;
using LuckNGold.World.Map.Components;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    /// <summary>
    /// Pointer that can select tiles and entities from the map.
    /// </summary>
    public Pointer Pointer { get; } = new();

    readonly PointerKeybindingsComponent _pointerKeybindingsComponent;

    public void ShowPointer()
    {
        Map.AddEntity(Pointer, Player.Position);
        SetFollowComponentTarget(Pointer);
        SadComponents.Remove(_playerKeybindingsComponent);
        SadComponents.Add(_pointerKeybindingsComponent);
    }

    public void HidePointer()
    {
        Map.RemoveEntity(Pointer);
        SetFollowComponentTarget(Player);
        if (_entityInfoWindow.IsVisible)
            HideEntityInfo();
        SadComponents.Remove(_pointerKeybindingsComponent);
        SadComponents.Add(_playerKeybindingsComponent);
        _followTargetComponent.CenterViewOnTarget();
    }

    void SetFollowComponentTarget(RogueLikeEntity target)
    {
        var mapFollowTargetComponent = Map.DefaultRenderer!.SadComponents
            .Where(c => c is FollowTargetComponent).First() as FollowTargetComponent;
        mapFollowTargetComponent!.Target = target;

        if (GameSettings.DebugEnabled)
        {
            var debugFollowTargetComponent = MapLayout.SadComponents
                .Where(c => c is FollowTargetComponent).First() as FollowTargetComponent;
            debugFollowTargetComponent!.Target = target;
        }
    }

    /// <summary>
    /// Toggles visibility of the entity info window.
    /// </summary>
    public void ToggleEntityInfo()
    {
        if (!_entityInfoWindow.IsVisible)
            ShowEntityInfo();
        else
            HideEntityInfo();
    }

    /// <summary>
    /// Shows entity info window.
    /// </summary>
    void ShowEntityInfo()
    {
        var entities = Map.GetEntitiesAt<RogueLikeEntity>(Pointer.Position).ToArray();
        if (entities.Length <= 1)
            return;

        // TODO: use selector to choose one.
        var entity = entities[1];

        if (entity.AllComponents.GetFirstOrDefault<IDescription>()
            is not IDescription descriptionComponent)
            return;

        string description = descriptionComponent.Description;
        if (description.Length == 0)
            return;

        string stateDescription = descriptionComponent.StateDescription;

        string name = entity.Name.Length > 0 ? entity.Name : "Entity";

        _entityInfoWindow.ShowDescription(name, description, stateDescription);
        _entityInfoWindow.Position = GetEntityInfoWindowPosition();

        _entityInfoWindow.Show();
    }

    /// <summary>
    /// Hides entity info pop up window.
    /// </summary>
    public void HideEntityInfo()
    {
        _entityInfoWindow.Hide();
    }

    /// <summary>
    /// Checks if there are any pop up windows showing.
    /// </summary>
    /// <returns>True if a pop up window is visible, false otherwise.</returns>
    bool EntityInfoVisible()
    {
        return _entityInfoWindow.IsVisible;
    }

    /// <summary>
    /// Hides entity info (if visible) or pointer.
    /// </summary>
    /// <remarks>Action for the escape button in pointer look mode.</remarks>
    public void HideEntityInfoOrPointer()
    {
        if (EntityInfoVisible())
            HideEntityInfo();
        else
            HidePointer();
    }

    Point GetEntityInfoWindowPosition()
    {
        Point windowPosition;

        var mapViewMidPoint = Map.DefaultRenderer!.Surface.ViewHeight / 2;
        var pointerPosition = Pointer.Position - Map.DefaultRenderer!.Surface.ViewPosition;
        bool windowPositionIsBelowEntity = pointerPosition.Y <= mapViewMidPoint;

        if (windowPositionIsBelowEntity)
        {
            pointerPosition = Pointer.Position + Direction.Down 
                - Map.DefaultRenderer!.Surface.ViewPosition;
            windowPosition = GetWindowPosition(pointerPosition);
        }
        else
        {
            windowPosition = GetWindowPosition(pointerPosition);
            windowPosition += (0, -_entityInfoWindow.Height);
        }

        return windowPosition;

        Point GetWindowPosition(Point pointerPosition)
        {
            var translatedPointerPosition = pointerPosition.TranslateFont(
            Map.DefaultRenderer!.FontSize, _entityInfoWindow.FontSize);
            var delta = (_entityInfoWindow.Width / 2, 0);
            return translatedPointerPosition - delta;
        }
    }

    void Pointer_OnPositionChanged(object? o, ValueChangedEventArgs<Point> e)
    {
        if (_entityInfoWindow.IsVisible)
            HideEntityInfo();
    }
}