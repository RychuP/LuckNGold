using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Components.Interfaces;

/// <summary>
/// It holds information about materials that an item entity is made of.
/// </summary>
internal interface IComposition
{
    IMaterial Material { get; }
}