using LuckNGold.Resources;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Draws clothes, robes, armour - layer 3.
    /// </summary>
    void DrawBodywear(RogueLikeEntity bodywear)
    {
        string fontName;
        int row = 0, col = 0;

        // Armour
        if (bodywear.Name.Contains(Strings.ArmourTag))
        {
            fontName = "armour";
        }

        // Clothes / Robes
        else
        {
            fontName = "clothes";

            // Robes
            if (bodywear.Name.Contains(Strings.RobeTag))
            {
                col = 1;
            }

            // Clothing
            else
            {
                col = 0;

                row = bodywear.Name.Contains(Strings.LinenTag) ? 0 :
                    throw new InvalidOperationException("Uknown clothing type.");
            }
        }

        SetLayerAppearance(OnionLayerName.Bodywear, fontName, row * 4, col * 3);
    }

    void EraseBodywear()
    {
        EraseLayer(OnionLayerName.Bodywear);
    }
}