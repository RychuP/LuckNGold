using LuckNGold.World.Items.Enums;

namespace LuckNGold.Visuals;

static class Colors
{
    // Map colors.
    public static readonly Color Floor = new(61, 37, 59);
    public static readonly Color Wall = new(37, 19, 26);

    // Gemstone colors.
    public static readonly Color Onyx = new(162, 145, 127);
    public static readonly Color Amber = new(222, 146, 80);
    public static readonly Color Emerald = new(102, 166, 74);
    public static readonly Color Ruby = new(213, 72, 78);
    public static readonly Color Diamond = new(40, 204, 223);

    public static readonly Dictionary<Gemstone, Color> FromGemstone = new()
    {
        {Gemstone.None, Color.Yellow },
        {Gemstone.Onyx, Onyx },
        {Gemstone.Amber, Amber },
        {Gemstone.Emerald, Emerald },
        {Gemstone.Ruby, Ruby },
        {Gemstone.Diamond, Diamond },
    };

    /// <summary>
    /// Color of the highlighted entity name in the entity info window.
    /// Command #1.
    /// </summary>
    public static readonly Color EntityHighlight = Color.LightGreen;

    /// <summary>
    /// Color of the highlighted entity state in the entity info window.
    /// Command #2.
    /// </summary>
    public static readonly Color StateHightlight = Color.Yellow;

    /// <summary>
    /// Tint of the explored area out of fov.
    /// </summary>
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