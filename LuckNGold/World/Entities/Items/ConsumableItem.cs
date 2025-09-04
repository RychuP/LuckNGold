namespace LuckNGold.World.Entities.Items;

internal abstract class ConsumableItem(string name, int glyph, Color color, int healthAmount, 
    int foodAmount, int drinkAmount, int volume, int weight) 
    : Item(name, glyph, color), IConsumable, ICarryable
{
    public int HealthAmount { get; init; } = healthAmount;
    public int FoodAmount { get; init; } = foodAmount;
    public int DrinkAmount { get; init; } = drinkAmount;
    public int Volume { get; init; } = volume;
    public int Weight { get; init; } = weight;
    public virtual string EffectDescription =>
        string.Empty;
    public bool IsHarmful =>
        HealthAmount < 0;
}