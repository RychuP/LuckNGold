using LuckNGold.Resources;
using LuckNGold.World.Turns;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays the time cost of the last action taken by the player.
/// </summary>
internal class ActionCost : InfoBox
{
    public ActionCost(TurnManager turnManager) : base("Action", Strings.ActionCostDescription)
    {
        turnManager.ActionAdded += (o, a) =>
        {
            if (a.Source.Name == "Player")
                Print(a.Time);
        };

        Print(0);
    }
}