using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be used in crafting recepies.
/// </summary>
/// <param name="material">Material that this component represents.</param>
/// <param name="amount">Amount of material in this ingredient.</param>
/// <param name="maxStackSize">Max number of instances of this entity that can
/// be stacked in one inventory slot.</param>
internal class IngredientComponent(Material material, int amount, int maxStackSize) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IIngredient
{
    /// <inheritdoc/>
    public int MaxStackSize => maxStackSize;

    /// <inheritdoc/>
    public Material Material => material;

    /// <inheritdoc/>
    public int Amount => amount;
}