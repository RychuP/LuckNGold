using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{

    /// <summary>
    /// Draws footwear - layer 4.
    /// </summary>
    void DrawFootwear(RogueLikeEntity footwear)
    {
        string fontName = "footwear";
        int row = 0, col = 0;

        SetLayerAppearance(OnionLayerName.Footwear, fontName, row * 4, col * 3);
    }

    void EraseFootwear()
    {
        EraseLayer(OnionLayerName.Footwear);
    }
}