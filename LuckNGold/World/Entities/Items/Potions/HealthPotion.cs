namespace LuckNGold.World.Entities.Items.Potions;

internal class HealthPotion() : Potion("Health", 4, Theme.HealingItem)
{
    public override string EffectDescription =>
        base.EffectDescription + $"{HealthAmount} HP";
}