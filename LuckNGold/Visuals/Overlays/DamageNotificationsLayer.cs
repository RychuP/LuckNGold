using LuckNGold.Visuals.Consoles;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Map;

namespace LuckNGold.Visuals.Overlays;

/// <summary>
/// Screen object that displays damage notifications.
/// </summary>
internal class DamageNotificationsLayer : ScreenObject
{
    public void DisplayDamageNotification(int amount, Point startPosition, Color color)
    {
        if (Parent is not GameScreen gameScreen || gameScreen.Map is not GameMap gameMap) return;

        // Create a new damage notification.
        var damageNotification = new DamageNotification($"{amount}", color);

        // Check font size is the same as the one currently set in map default renderer.
        if (damageNotification.FontSize != gameMap.DefaultRenderer!.FontSize)
            damageNotification.FontSize = gameMap.DefaultRenderer!.FontSize;

        // Set position for the notification.
        startPosition -= gameMap.DefaultRenderer!.Surface.ViewPosition;
        damageNotification.Position = startPosition;

        // Display notification.
        Children.Add(damageNotification);
    }
}