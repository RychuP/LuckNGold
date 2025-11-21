namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IAttackDamage
{
    IPhysicalDamage PhysicalDamage { get; }
    IElementalDamage ElementalDamage { get; }
}