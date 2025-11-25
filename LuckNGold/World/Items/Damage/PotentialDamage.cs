using GoRogue.Random;
using LuckNGold.World.Items.Damage.Interfaces;

namespace LuckNGold.World.Items.Damage;

record struct PotentialDamage(int MinDamage, int MaxDamage) : IPotentialDamage
{
    public static readonly PotentialDamage None = new(0, 0);
    public readonly int Resolve() =>
        GlobalRandom.DefaultRNG.NextInt(MinDamage, MaxDamage + 1);
}