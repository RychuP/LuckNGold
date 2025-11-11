using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Items.Primitives;

record struct BaseDamage(int MinDamage, int MaxDamage) : IBaseDamage
{
    public static readonly BaseDamage None = new(0, 0);
}