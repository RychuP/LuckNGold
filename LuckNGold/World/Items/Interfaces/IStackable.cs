namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be stacked so that many instances can occupy one inventory slot.
/// </summary>
internal interface IStackable : ICarryable
{
    int MaxStackSize { get; }
}