using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It has a name, race and other identifying features.
/// </summary>
interface IIdentity
{
    event EventHandler<ValueChangedEventArgs<Appearance>>? AppearanceChanged;
    string Name { get; }
    Race Race { get; }
    Appearance Appearance { get; }
}