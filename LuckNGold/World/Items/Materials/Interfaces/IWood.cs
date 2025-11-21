using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

internal interface IWood : IMaterial
{
    WoodType WoodType { get; }
}