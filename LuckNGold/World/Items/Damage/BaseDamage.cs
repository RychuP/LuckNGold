using LuckNGold.World.Items.Damage.Interfaces;

namespace LuckNGold.World.Items.Damage;

record struct BaseDamage(int MinDamage, int MaxDamage) : IBaseDamage
{
    public static readonly BaseDamage None = new(0, 0);
}