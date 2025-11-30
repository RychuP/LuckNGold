using LuckNGold.Config;
using LuckNGold.Visuals.Consoles;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Map;

namespace LuckNGold.Visuals.Overlays;

/// <summary>
/// Screen object that displays damage notifications.
/// </summary>
internal class DamageNotificationsLayer : ScreenObject
{
    public DamageNotificationsLayer()
    {
        for (int i = 0; i < 20; i++)
            Children.Add(new DamageNotification(2));
    }

    public void DisplayNotification(int amount, Point startPosition, Color color)
    {
        if (Parent is not GameScreen gameScreen || gameScreen.Map is not GameMap gameMap) return;

        // Normalize amount to a 2 digit number.
        string text = $"{amount:D2}";
        if (text.Length > 2)
            text = text[..2];

        // Get a free damage notification.
        var damageNotification = Children
            .Cast<DamageNotification>()
            .Where(c => !c.IsVisible && c.Width == text.Length)
            .FirstOrDefault();

        // Skip when too many notifications are being displayed.
        if (damageNotification is null) return;

        var targetFontSize = gameMap.DefaultRenderer!.FontSize / 2;
        if (targetFontSize.X < GameSettings.FontSize.X)
            targetFontSize = GameSettings.FontSize;
        damageNotification.FontSize = targetFontSize;

        // Calculate screen position of the source entity.
        startPosition -= gameMap.DefaultRenderer!.Surface.ViewPosition;
        
        // Translate position to default font size.
        startPosition = startPosition.TranslateFont(gameMap.DefaultRenderer!.FontSize, targetFontSize);

        // Set damage notification position.
        damageNotification.Position = startPosition;

        // Display notification.
        damageNotification.Show(text, color);
    }

    public void UpdateNotificationsPosition(Direction direction)
    {
        if (Parent is not GameScreen gameScreen || gameScreen.Map is not GameMap gameMap) return;

        var visibleNotifications = Children
            .Cast<DamageNotification>()
            .Where(c => c.IsVisible);

        foreach (var notification in visibleNotifications)
        {
            var position = notification.Position.TranslateFont(notification.FontSize,
                gameMap.DefaultRenderer!.FontSize);
            position += direction;
            notification.Position = position.TranslateFont(gameMap.DefaultRenderer!.FontSize,
                notification.FontSize);
        }
    }
}