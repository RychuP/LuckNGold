using GoRogue.Random;
using LuckNGold.World.Items.Defences.Interfaces;

namespace LuckNGold.World.Items.Defences;

record struct PotentialProtection(int MinProtection, int MaxProtection) : IPotentialProtection
{
    public static readonly PotentialProtection None = new(0, 0);

    public readonly int Resolve() =>
        GlobalRandom.DefaultRNG.NextInt(MinProtection, MaxProtection + 1);
}