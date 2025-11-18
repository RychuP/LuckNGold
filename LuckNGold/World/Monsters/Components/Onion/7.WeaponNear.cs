using LuckNGold.World.Monsters.Enums;

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
}