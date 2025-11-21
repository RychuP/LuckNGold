using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws weapon near - layer 7.
    /// </summary>
    void DrawWeaponNear(string fontName, int row, int col)
    {
        SetLayerAppearance(OnionLayerName.WeaponNear, $"{fontName}-near", row * 4, col * 3);
    }

    void EraseWeaponNear()
    {
        EraseLayer(OnionLayerName.WeaponNear);
    }

    void DrawWeapon(RogueLikeEntity weapon)
    {
        var composition = weapon.AllComponents.GetFirst<IComposition>();
        var material = composition.Material;
        string fontName = "weapons-1";
        int row = 0, col = 0;

        if (weapon.AllComponents.Contains<IMeleeAttack>())
        {
            if (weapon.Name.Contains("Sword"))
            {
                row = weapon.Name.Contains("Arming") ? 0 :
                    weapon.Name.Contains("Gladius") ? 0 :
                    weapon.Name.Contains("Scimitar") ? 0 :
                    throw new InvalidOperationException("Unknown sword type.");

                col = weapon.Name.Contains("Arming") ? 0 :
                    weapon.Name.Contains("Gladius") ? 1 :
                    weapon.Name.Contains("Scimitar") ? 2 :
                    throw new InvalidOperationException("Unknown sword type.");
            }
        }

        DrawWeaponFar(fontName, row, col);
        DrawWeaponNear(fontName, row, col);
        DrawRightWeaponHand(row, col);
    }

    void EraseWeapon()
    {
        EraseWeaponFar();
        EraseWeaponNear();
        DrawRightEmptyHand();
    }
}