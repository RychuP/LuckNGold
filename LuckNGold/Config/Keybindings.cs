namespace LuckNGold.Config;

static class Keybindings
{
    public static bool ViMotionsEnabled = true;
    public static bool ArrowMotionsEnabled = true;
    public static bool NumpadMotionsEnabled = true;
    public static bool FPSMotionsEnabled = true;

    public static Dictionary<string, bool> MotionSchemes = new()
    {
        { "ViMotionsEnabled", true }
    };
}