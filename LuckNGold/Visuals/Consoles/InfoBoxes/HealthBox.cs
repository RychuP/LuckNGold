using LuckNGold.Resources;
using LuckNGold.World.Monsters.Components.Interfaces;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

internal class HealthBox : InfoBox
{
    public HealthBox(IHealth health) : base("Health", Strings.HealthBoxDescription)
    {
        health.HPChanged += (o, e) => Print(e.NewValue);
        Print(health.HP);
    }
}