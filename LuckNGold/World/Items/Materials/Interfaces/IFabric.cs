using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

internal interface IFabric : IMaterial
{
    FabricType FabricType { get; }
}