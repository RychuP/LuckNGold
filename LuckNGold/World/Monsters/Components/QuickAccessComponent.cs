namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Additional, always visible inventory with keyboard shortcuts
/// meant to be attached to the player entity.
/// </summary>
class QuickAccessComponent() : InventoryComponent(Max)
{
    /// <summary>
    /// Max number of slots in the quick access inventory.
    /// </summary>
    public const int Max = 10;
}