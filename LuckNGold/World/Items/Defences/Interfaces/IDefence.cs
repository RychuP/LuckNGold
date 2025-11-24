namespace LuckNGold.World.Items.Defences.Interfaces;

interface IDefence
{
    IList<IPhysicalProtection> PhysicalProtections { get; }
    IList<IElementalProtection> ElementalProtections { get; }
}