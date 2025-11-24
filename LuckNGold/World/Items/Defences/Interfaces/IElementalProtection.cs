using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Defences.Interfaces;

interface IElementalProtection
{
    ElementalEffectType EffectType { get; }
    IBaseProtection BaseProtection { get; }
}