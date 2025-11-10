using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be used in crafting recepies. 
/// </summary>
internal interface IIngredient : IStackable
{
    /// <summary>
    /// Amount of <see cref="Material"/> this ingredient holds.
    /// </summary>
    int Amount { get; }

    /// <summary>
    /// Material that this ingredient represents.
    /// </summary>
    Material Material { get; }
}