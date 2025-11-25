using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Damage;

record struct ElementalDamage(ElementalEffectType EffectType, int Amount) : IElementalDamage
{
    public static readonly ElementalDamage None = new(ElementalEffectType.None, 0);
}