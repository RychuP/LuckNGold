using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be used in crafting recepies.
/// </summary>
internal interface IIngredient : IStackable
{
    Material Material { get; }
}