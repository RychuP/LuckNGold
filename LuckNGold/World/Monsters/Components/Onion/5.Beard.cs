using LuckNGold.Resources;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates beard - layer 5.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    void DrawBeard(RogueLikeEntity? headwear = null)
    {
        Race race = IdentityComponent.Race;
        if (!race.CanGrowBeard) return;

        var appearance = IdentityComponent.Appearance;
        if (appearance.BeardStyle == BeardStyle.None)
        {
            EraseLayer(OnionLayerName.Beard);
            return;
        }

        string raceType = GetRaceTypeText(race);
        var beardVariant = (int)appearance.BeardColor + 1;
        string fontName = $"race-{raceType}-beards-{beardVariant}";
        int row, col = 0;

        // Helmet or hood and beard.
        if (headwear != null)
        {
            col = headwear.Name.Contains(Strings.HelmetTag) ? 2 : 1;
        }

        row = appearance switch
        {
            { BeardStyle: BeardStyle.Circle, IsAngry: true } => 1,
            { BeardStyle: BeardStyle.Boxed, IsAngry: false } => 2,
            { BeardStyle: BeardStyle.Boxed, IsAngry: true } => 3,
            _ => 0 // circle, not angry
        };

        SetLayerAppearance(OnionLayerName.Beard, fontName, row * 4, col * 3);
    }
}