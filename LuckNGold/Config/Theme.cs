using LuckNGold.World.Items.Enums;

namespace LuckNGold.Config;

static class Theme
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

    public static readonly Dictionary<Gemstone, Color> GemstoneColors = new()
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
    /// </summary>
    public const string EntityHighlight = "LightGreen";

    /// <summary>
    /// Color of the highlighted entity state in the entity info window.
    /// </summary>
    public const string StateHightlight = "Yellow";

    /// <summary>
    /// Tint of the explored area out of fov.
    /// </summary>
    public static readonly Color Tint = new(0.05f, 0.05f, 0.05f, 0.5f);

    /// <summary>
    /// Color of the border around items in the inventory window.
    /// </summary>
    public static readonly Color SelectorBorder = Color.DarkGray;

    /// <summary>
    /// Color of the digit on the bottom line of the slot border.
    /// </summary>
    public static readonly Color SlotDigit = Color.LightBlue;
}