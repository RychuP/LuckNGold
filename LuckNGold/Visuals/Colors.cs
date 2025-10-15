using LuckNGold.World.Items.Enums;

namespace LuckNGold.Visuals;

static class Colors
{
    public static readonly Color Floor = new(61, 37, 59);
    public static readonly Color Wall = new(37, 19, 26);

    public static readonly Color Onyx = new(162, 145, 127);
    public static readonly Color Amber = new(222, 146, 80);
    public static readonly Color Emerald = new(102, 166, 74);
    public static readonly Color Ruby = new(213, 72, 78);
    public static readonly Color Diamon = new(40, 204, 223);

    public static readonly Dictionary<Gemstone, Color> FromGemstone = new()
    {
        {Gemstone.None, Color.DarkGray },
        {Gemstone.Onyx, Color.LightGray },
        {Gemstone.Amber, Color.LightSalmon },
        {Gemstone.Emerald, Color.LightGreen },
        {Gemstone.Ruby, Color.LightPink },
        {Gemstone.Diamond, Color.LightBlue },
    };

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