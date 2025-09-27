namespace LuckNGold.Visuals;

static class Colors
{
    public static readonly Color Floor = new(61, 37, 59);
    public static readonly Color Wall = new(37, 19, 26);

    // tint of the explored area out of fov
    public static readonly Color Tint = new(0.05f, 0.05f, 0.05f, 0.5f);

    /// <summary>
    /// Color of the border around items in the inventory window.
    /// </summary>
    public static readonly Color SelectorBorder = Color.DarkGray;

    /// <summary>
    /// Color of the item selector number in the inventory window.
    /// </summary>
    public static readonly Color SelectorNumber = Color.LightBlue;
}