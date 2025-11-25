namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IPotentialDamage
{
    int MinDamage { get; }
    int MaxDamage { get; }
    int Resolve();
}