namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can be opened and closed.
/// </summary>
internal interface IOpenable : IInteractable
{
    bool IsOpen { get; }
    bool Open();
    bool Close();
}