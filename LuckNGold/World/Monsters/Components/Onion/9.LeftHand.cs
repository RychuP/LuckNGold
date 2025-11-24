using LuckNGold.Generation.Items.Weapons;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws left empty hand - layer 9.
    /// </summary>
    void DrawLeftEmptyHand()
    {
        Race race = IdentityComponent.Race;

        string raceType = GetRaceTypeText(race);
        string skinTone = GetSkinToneText(race);
        string fontName = $"race-{raceType}-base{skinTone}";

        int row = race == Race.Human ? 17 :
            race == Race.Skeleton ? 24 : 0;

        int col = 0;

        SetLayerAppearance(OnionLayerName.LeftHand, fontName, row * 4, col * 3);
    }

    /// <summary>
    /// Draws shield near - layer 9.
    /// </summary>
    void DrawShieldNear(string fontName, int row, int col)
    {
        SetLayerAppearance(OnionLayerName.LeftHand, $"{fontName}-near", row * 4, col * 3);
    }

    void DrawShield(RogueLikeEntity shield)
    {
        var composition = shield.AllComponents.GetFirst<IComposition>();
        var material = composition.Material;
        string fontName = "shields-1";
        string shieldType = shield.Name.Split(' ')[1];
        (int row, int col) = shieldType switch
        {
            _ => (0, 0), // Bandit
        };
        DrawShieldNear(fontName, row, col);
        DrawShieldFar(fontName, row, col);
    }

    void EraseShield()
    {
        EraseShieldFar();
        DrawLeftEmptyHand();
    }
}