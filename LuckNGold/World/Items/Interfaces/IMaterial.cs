using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It is made of a material.
/// </summary>
internal interface IMaterial
{
    Material Material { get; }
}