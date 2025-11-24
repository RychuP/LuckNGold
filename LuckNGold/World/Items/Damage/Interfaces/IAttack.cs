namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IAttack
{
    IPhysicalDamage PhysicalDamage { get; }
    IElementalDamage ElementalDamage { get; }
}