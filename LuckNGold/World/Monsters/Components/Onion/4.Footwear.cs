using LuckNGold.Resources;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{

    /// <summary>
    /// Draws footwear - layer 4.
    /// </summary>
    void DrawFootwear(RogueLikeEntity footwear, RogueLikeEntity? bodywear)
    {
        string fontName = "footwear";
        int col = bodywear != null && !bodywear.Name.Contains(Strings.RobeTag) ? 1 : 0;
        int row = 0;

        SetLayerAppearance(OnionLayerName.Footwear, fontName, row * 4, col * 3);
    }

    void EraseFootwear()
    {
        EraseLayer(OnionLayerName.Footwear);
    }
}