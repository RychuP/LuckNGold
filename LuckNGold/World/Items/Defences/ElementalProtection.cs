using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences;

record struct ElementalProtection(ElementalEffectType EffectType, int Amount) : IElementalProtection
{
    public static readonly ElementalProtection None = new(ElementalEffectType.None, 0);
}