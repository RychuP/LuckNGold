using LuckNGold.World.Items.Defences.Interfaces;

namespace LuckNGold.World.Items.Defences;

record struct Defence(IList<IPotentialPhysicalProtection> PotentialPhysicalProtections,
    IList<IPotentialElementalProtection> PotentialElementalProtections) : IDefence
{
    public static readonly Defence None = new([], []);
}