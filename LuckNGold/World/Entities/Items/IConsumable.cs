namespace LuckNGold.World.Entities.Items;

internal interface IConsumable
{
    // in hp points
    int HealthAmount { get; }

    string EffectDescription { get; }

    bool IsHarmful { get; }
}