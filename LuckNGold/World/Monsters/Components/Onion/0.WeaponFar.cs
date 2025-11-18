using LuckNGold.Generation.Items.Weapons;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws weapon far - layer 0.
    /// </summary>
    void DrawWeaponFar(string fontName, int row, int col)
    {
        SetLayerAppearance(OnionLayerName.WeaponFar, $"{fontName}-far", row * 4, col * 3);
    }

    void EraseWeaponFar()
    {
        EraseLayer(OnionLayerName.WeaponFar);
    }
}