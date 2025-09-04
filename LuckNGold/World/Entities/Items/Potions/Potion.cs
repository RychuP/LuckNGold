namespace LuckNGold.World.Entities.Items.Potions;

internal abstract class Potion(string name, int healthAmount, Color color, int weight = 150, int volume = 200) 
    : ConsumableItem(name + " Potion", '!', color, healthAmount, 10, 0, volume, weight)
{
    public override string EffectDescription =>
        $"drink a {Name.ToLower()} and {(IsHarmful ? "loose" : "recover")} ";
}