using LuckNGold.World.Furniture.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// Interface for components that can unlock entities with <see cref="ILockable"/> components.
/// </summary>
internal interface IUnlocker : IUsable
{
    /// <summary>
    /// Quality of the <see cref="IUnlocker"/>. 
    /// Will open <see cref="ILockable"/> with a matched <see cref="ILockable.Difficulty"/>.
    /// </summary>
    Quality Quality { get; }
}