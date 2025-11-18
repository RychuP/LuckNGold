using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates hair, helmet - layer 6.
    /// </summary>
    void UpdateHeadwearLayer()
    {
        // Check if the race can grow hair at all.
        Race race = IdentityComponent.Race;
        if (!race.CanGrowHair) return;

        var appearance = IdentityComponent.Appearance;
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Helmet on the head.
        if (EquipmentComponent.Head != null)
        {

        }

        // Hair only. Applies to humans and elves. All other races don't grow hair.
        else
        {
            // Shaved or bald head don't need hair.
            if (appearance.HairStyle == HairStyle.Bald ||
                appearance.HairStyle == HairStyle.Shaved)
            {
                EraseLayer(OnionLayerName.Headwear);
                return;
            }

            string raceType = GetRaceTypeText(race);
            string skinTone = race.RaceType == RaceType.Elf && race.SkinTone == SkinTone.Dark ?
                "dark-" : string.Empty;
            var hairVariant = (int)appearance.HairColor + 1;
            fontName = $"race-{skinTone}{raceType}-hair-{hairVariant}";

            column = (int)appearance.HairCut;
            row = appearance.HairStyle switch
            {
                HairStyle.Long => 1,
                _ => 0
            };
        }

        SetLayerAppearance(OnionLayerName.Headwear, fontName, row * 4, column * 3);
    }
}