using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Materials.Interfaces;

internal interface ILeather : IMaterial
{
    LeatherType LeatherType { get; }
}