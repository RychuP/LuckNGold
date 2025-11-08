using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates beard (5) layer.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    void UpdateBeardLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var identityComponent = Parent.AllComponents.GetFirst<IdentityComponent>();
        Race race = identityComponent.Race;
        if (!race.CanGrowBeard) return;

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        var appearance = identityComponent.Appearance;

        if (appearance.BeardStyle == BeardStyle.None)
        {
            EraseLayer(OnionLayerName.Beard);
            return;
        }

        string raceType = race.RaceType.ToString().ToLower();
        var beardVariant = (int)appearance.BeardColor + 1;
        string fontName = $"race-{raceType}-beards-{beardVariant}";
        int row = 0, column = 0;

        // Helmet or hood and beard.
        if (equipment.Head != null)
        {

        }

        // Beard only. Applies to humans only. None other race can grow beards.
        else
        {
            
            row = appearance switch
            {
                { BeardStyle: BeardStyle.Circle, IsAngry: true } => 1,
                { BeardStyle: BeardStyle.Boxed, IsAngry: false } => 2,
                { BeardStyle: BeardStyle.Boxed, IsAngry: true } => 3,
                _ => 0 // circle, not angry
            };
        }

        SetLayerAppearance(OnionLayerName.Beard, fontName, row * 4, column * 3);
    }
}