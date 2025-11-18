using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws left empty hand - layer 9.
    /// </summary>
    void DrawLeftEmptyHand()
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
            column = 0;
        }

        SetLayerAppearance(OnionLayerName.LeftHand, fontName, row * 4, column * 3);
    }

    /// <summary>
    /// Draws shield near - layer 9.
    /// </summary>
    void DrawShieldNear()
    {

    }
}