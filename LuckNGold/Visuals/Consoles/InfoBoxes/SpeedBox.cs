using LuckNGold.Resources;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays amount of time points gained each turn.
/// </summary>
internal class SpeedBox : InfoBox
{
    public SpeedBox() : base("Speed", Strings.SpeedBoxDescription)
    {
        Print(100);
    }
}