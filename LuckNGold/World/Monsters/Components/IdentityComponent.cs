using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that can be identified by their race, appearance and name.
/// </summary>
/// <param name="name">Unique name of the parent entity.</param>
/// <param name="race">Race of the parent entity.</param>
internal class IdentityComponent(string name, Race race) : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IIdentity
{
    public event EventHandler<ValueChangedEventArgs<Appearance>>? AppearanceChanged;
    public string Name { get; } = name;
    public Race Race { get; } = race;

    public Appearance _appearance = new();
    public Appearance Appearance
    {
        get => _appearance;
        set
        {
            if (_appearance == value) return;
            var oldValue = _appearance;
            _appearance = value;
            OnAppearanceChanged(oldValue, value);
        }
    }

    void OnAppearanceChanged(Appearance oldValue, Appearance newValue)
    {
        var args = new ValueChangedEventArgs<Appearance>(oldValue, newValue);
        AppearanceChanged?.Invoke(this, args);
    }
}