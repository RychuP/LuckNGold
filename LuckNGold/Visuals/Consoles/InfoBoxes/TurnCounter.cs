using LuckNGold.Resources;
using LuckNGold.World.Turns;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays number of turns taken so far.
/// </summary>
internal class TurnCounter : InfoBox
{
    public TurnCounter(TurnManager turnManager) : base("Turns", Strings.TurnCounterDescription)
    {
        turnManager.TurnCounterChanged += (o, e) => Print(turnManager.TurnCounter);
        Print(turnManager.TurnCounter);
    }
}