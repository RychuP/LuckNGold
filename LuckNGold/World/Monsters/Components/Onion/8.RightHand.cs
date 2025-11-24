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

        string raceType = GetRaceTypeText(race);
        string skinTone = GetSkinToneText(race);
        string fontName = $"race-{raceType}-base{skinTone}";

        int row = race == Race.Human ? 17 :
            race == Race.Skeleton ? 25 : 0;

        int col = race == Race.Human ? 2 : 0;

        SetLayerAppearance(OnionLayerName.RightHand, fontName, row * 4, col * 3);
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