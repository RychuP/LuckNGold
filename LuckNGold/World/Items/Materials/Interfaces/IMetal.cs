using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

internal interface IMetal : IMaterial
{
    MetalType MetalType { get; }
}