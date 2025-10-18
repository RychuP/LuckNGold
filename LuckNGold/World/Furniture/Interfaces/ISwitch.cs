using LuckNGold.World.Furniture.Enums;

namespace LuckNGold.World.Furniture.Interfaces;

/// <summary>
/// It can switch between on and off state.
/// </summary>
internal interface ISwitch : IInteractable
{
    /// <summary>
    /// Current state of <see cref="ISwitch"/>.
    /// </summary>
    SwitchState State { get; }

    /// <summary>
    /// Alternates between on and off position.
    /// </summary>
    void Toggle();
}