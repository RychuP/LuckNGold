using LuckNGold.World.Furniture.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Interface for components that can lock/unlock <see cref="ILockable"/> components.
/// </summary>
internal interface IKey
{
    KeyType Type { get; }
}