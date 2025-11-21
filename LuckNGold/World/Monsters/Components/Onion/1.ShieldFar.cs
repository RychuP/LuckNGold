using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws shield far - layer 1.
    /// </summary>
    void DrawShieldFar(string fontName, int row, int col)
    {
        SetLayerAppearance(OnionLayerName.ShieldFar, $"{fontName}-far", row * 4, col * 3);
    }

    void EraseShieldFar()
    {
        EraseLayer(OnionLayerName.ShieldFar);
    }
}