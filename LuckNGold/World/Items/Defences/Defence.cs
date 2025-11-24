using LuckNGold.World.Items.Defences.Interfaces;

namespace LuckNGold.World.Items.Defences;

record struct Defence(IList<IPhysicalProtection> PhysicalProtections,
    IList<IElementalProtection> ElementalProtections) : IDefence
{ }