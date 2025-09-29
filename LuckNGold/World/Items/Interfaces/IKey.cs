using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// Interface for components that can unlock entities with <see cref="ILockable"/> components.
/// </summary>
internal interface IKey : IUsable
{
    /// <summary>
    /// Color of this key.
    /// </summary>
    KeyColor KeyColor { get; }
}