using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

/// <summary>
/// It is a material that item entities can be made of.
/// </summary>
interface IMaterial
{
    MaterialType MaterialType { get; }
}