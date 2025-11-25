using LuckNGold.World.Items.Damage.Interfaces;

namespace LuckNGold.World.Items.Damage;

record struct Attack(PotentialPhysicalDamage PotentialPhysicalDamage,
    PotentialElementalDamage PotentialElementalDamage) : IAttack
{
    public static readonly Attack None = 
        new(PotentialPhysicalDamage.None, PotentialElementalDamage.None);
}