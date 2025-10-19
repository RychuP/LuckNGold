namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can switch between on and off state.
/// </summary>
internal interface ISwitch : IInteractable
{
    /// <summary>
    /// Current state.
    /// </summary>
    bool IsOn { get; }
}