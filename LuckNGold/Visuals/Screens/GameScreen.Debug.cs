using LuckNGold.Config;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Overlays;

namespace LuckNGold.Visuals.Screens;

partial class GameScreen
{
    public static MapLayout MapLayout { get; } = new();

    void AddDebugOverlays()
    {
        MapLayout.DrawOverlay(Map);
        MapLayout.SadComponents.Clear();
        FollowTargetComponent followTargetComponent = new(Player);
        MapLayout.SadComponents.Add(followTargetComponent);
        Children.Add(MapLayout);
        Children.Add(DebugConsole.Instance);
    }
}