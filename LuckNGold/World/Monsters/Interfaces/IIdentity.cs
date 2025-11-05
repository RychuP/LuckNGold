namespace LuckNGold.World.Monsters.Interfaces;

/// <summary>
/// It has a name, race and other identifying features.
/// </summary>
interface IIdentity
{
    event EventHandler<ValueChangedEventArgs<IAppearance>>? AppearanceChanged;
    string Name { get; }
    IRace Race { get; }
    IAppearance Appearance { get; }
}