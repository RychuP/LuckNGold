using GoRogue.Random;
using LuckNGold.World.Items.Defences.Interfaces;

namespace LuckNGold.World.Items.Defences;

record struct BaseProtection(int MinProtection, int MaxProtection) : IBaseProtection
{
    public readonly int Resolve() =>
        GlobalRandom.DefaultRNG.NextInt(MinProtection, MaxProtection + 1);
}