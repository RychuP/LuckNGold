using LuckNGold.Config;
using LuckNGold.Visuals.Consoles.InfoBoxes;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Surface that displays player health, coins and other at-a-glance information.
/// </summary>
internal class StatusSurface : ScreenSurface
{
    readonly ShapeParameters _borderShapeParams;

    bool _borders = true;
    /// <summary>
    /// Whether to draw borders around info boxes or not.
    /// </summary>
    public bool Borders
    {
        get => _borders;
        set
        {
            if (_borders == value) return;
            _borders = value;
            Organise();
        }
    }

    public StatusSurface() : base(GameSettings.Width, 4)
    {
        _borderShapeParams = ShapeParameters.CreateStyledBoxThin(Theme.SlotBorder);
        Children.CollectionChanged += (o, e) => Organise();
    }

    void Organise()
    {
        int spacingX = Borders ? 1 : 0;
        int totalWidth = spacingX;
        int childCount = 0;
        var children = Children
            .Cast<InfoBox>()
            .ToArray();

        Surface.Clear();

        // Calculate total width of all children and spacing.
        foreach (var child in children)
        {
            int tempWidth = totalWidth + child.Width + spacingX;
            if (tempWidth > Width)
                break;
            else
            {
                totalWidth = tempWidth;
                childCount++;
            }
        }

        // Calculate the centered position for the children.
        int x = ((Width - totalWidth) / 2);
        if (Borders)
        {
            var border = new Rectangle(x, 0, totalWidth, Height);
            Surface.DrawBox(border, _borderShapeParams);
        }
        x += spacingX;

        // Set positions and draw borders if necessary.
        for (int i = 0; i < childCount; i++)
        {
            var child = children[i];
            child.Position = (x, 1);
            x += child.Width;
            if (Borders && i != childCount - 1)
            {
                DrawLine(x);
                x += spacingX;
            }
        }

        Surface.ConnectLines();
    }

    void DrawLine(int x)
    {
        Point start = (x, 0);
        Point end = (x, Height - 1);
        Surface.DrawLine(start, end, 179, Theme.SlotBorder);
    }
}