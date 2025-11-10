namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be stacked so that many can occupy one inventory slot.
/// </summary>
internal interface IStackable : ICarryable
{
    /// <summary>
    /// Maximum amount per stack in one slot of the inventory.
    /// </summary>
    int MaxStackSize { get; }
}