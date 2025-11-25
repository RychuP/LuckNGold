namespace LuckNGold.World.Items.Defences.Interfaces;

interface IDefence
{
    IList<IPotentialPhysicalProtection> PotentialPhysicalProtections { get; }
    IList<IPotentialElementalProtection> PotentialElementalProtections { get; }
}