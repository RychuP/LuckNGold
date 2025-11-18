using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws right empty hand - layer 8.
    /// </summary>

    void DrawRightEmptyHand()
    {
        Race race = IdentityComponent.Race;
        string fontName = "race-";
        int row = 0, column = 0;

        string raceType = GetRaceTypeText(race);
        string skinTone = GetSkinToneText(race);

        if (race == Race.Human)
        {
            fontName += $"{raceType}-base{skinTone}";
            row = 17;
            column = 2;
        }

        SetLayerAppearance(OnionLayerName.RightHand, fontName, row * 4, column * 3);
    }

    /// <summary>
    /// Draws right weapon hand - layer 8.
    /// </summary>
    void DrawRightWeaponHand(int row, int col)
    {
        Race race = IdentityComponent.Race;
        string raceType = GetRaceTypeText(race);
        string skinTone = GetSkinToneText(race);
        string fontName = $"race-{raceType}-weapon-hands{skinTone}";
        SetLayerAppearance(OnionLayerName.RightHand, fontName, row * 4, col * 3);
    }
}