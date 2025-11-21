using LuckNGold.World.Furnitures.Interfaces;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Components.Interfaces;

/// <summary>
/// It can unlock entities with <see cref="ILockable"/>.
/// </summary>
internal interface IUnlocker : IUsable
{
    /// <summary>
    /// Quality of the <see cref="IUnlocker"/>. 
    /// Will open <see cref="ILockable"/> with a matched <see cref="ILockable.Difficulty"/>.
    /// </summary>
    Quality Quality { get; }
}