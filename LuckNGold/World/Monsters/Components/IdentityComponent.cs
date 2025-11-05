using LuckNGold.World.Monsters.Interfaces;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

internal class IdentityComponent(string name, Race race) : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IIdentity
{
    public event EventHandler<ValueChangedEventArgs<IAppearance>>? AppearanceChanged;
    public string Name { get; } = name;
    public IRace Race { get; } = race;

    IAppearance _appearance = new Appearance();
    public IAppearance Appearance
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

    void OnAppearanceChanged(IAppearance oldValue, IAppearance newValue)
    {
        var args = new ValueChangedEventArgs<IAppearance>(oldValue, newValue);
        AppearanceChanged?.Invoke(this, args);
    }
}