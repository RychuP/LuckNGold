namespace LuckNGold.World.Items.Interfaces;

internal interface IAttackDamage
{
    IPhysicalDamage PhysicalDamage { get; }
    IElementalDamage ElementalDamage { get; }
}