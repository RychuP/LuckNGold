namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can switch between on and off state.
/// </summary>
internal interface ISwitch : IInteractable
{
    event EventHandler? StateChanged;
    event EventHandler<ValueChangedEventArgs<bool>>? StateChanging;

    /// <summary>
    /// Current state.
    /// </summary>
    bool IsOn { get; }

    /// <summary>
    /// Can be called in response to <see cref="StateChanging"/> event 
    /// to stop state change from happenning.
    /// </summary>
    void StopStateChanging();
}