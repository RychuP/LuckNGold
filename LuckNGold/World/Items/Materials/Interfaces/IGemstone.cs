using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

internal interface IGemstone : IMaterial
{
    GemstoneType GemstoneType { get; }
    Color Color { get; }
}