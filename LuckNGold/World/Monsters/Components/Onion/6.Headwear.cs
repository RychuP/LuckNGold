using LuckNGold.Generation.Items.Weapons;
using LuckNGold.Resources;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws hair - layer 6.
    /// </summary>
    void DrawHair()
    {
        Race race = IdentityComponent.Race;
        var appearance = IdentityComponent.Appearance;

        // Shaved or bald head don't need hair.
        if (appearance.HairStyle == HairStyle.Bald ||
            appearance.HairStyle == HairStyle.Shaved)
        {
            EraseHeadwear();
            return;
        }

        string raceType = GetRaceTypeText(race);
        string skinTone = race.RaceType == RaceType.Elf && race.SkinTone == SkinTone.Dark ?
            "dark-" : string.Empty;
        var hairVariant = (int)appearance.HairColor + 1;
        string fontName = $"race-{skinTone}{raceType}-hair-{hairVariant}";

        int col = (int)appearance.HairCut;
        int row = appearance.HairStyle switch
        {
            HairStyle.Long => 1,
            _ => 0
        };

        SetLayerAppearance(OnionLayerName.Headwear, fontName, row * 4, col * 3);
    }

    void DrawHeadwear(RogueLikeEntity headwear)
    {
        var composition = headwear.AllComponents.GetFirst<IComposition>();
        var material = composition.Material;
        string fontName = "helmets-1";
        int row = 0, col = 0;

        if (headwear.Name.Contains(Strings.HelmetTag))
        {
            var helmetType = headwear.Name.Split(' ')[1];
            (row, col) = helmetType switch
            {
                _ => (0, 0),    // Bandit
            };
        }

        SetLayerAppearance(OnionLayerName.Headwear, fontName, row * 4, col * 3);
    }

    void EraseHeadwear()
    {
        EraseLayer(OnionLayerName.Headwear);
    }
}