namespace LuckNGold.World.Items.Damage.Interfaces;

internal interface IAttack
{
    PotentialPhysicalDamage PotentialPhysicalDamage { get; }
    PotentialElementalDamage PotentialElementalDamage { get; }
}