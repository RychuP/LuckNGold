using LuckNGold.Resources;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates clothes, robes, armour - layer 3.
    /// </summary>
    void UpdateBodywearLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Wearing clothes / armour.
        if (equipment.Body is RogueLikeEntity bodyEquipment)
        {
            // Armour
            if (bodyEquipment.Name.Contains(Strings.ArmourTag))
            {
                fontName = "armour";
            }

            // Clothes / Robes
            else
            {
                fontName = "clothes";

                // Robes
                if (bodyEquipment.Name.Contains(Strings.RobeTag))
                {
                    column = 1;
                }

                // Clothing
                else
                {
                    column = 0;
                    
                }
            }
        }

        // Not wearing anything.
        else
        {
            EraseLayer(OnionLayerName.Bodywear);
            return;
        }

        SetLayerAppearance(OnionLayerName.Bodywear, fontName, row * 4, column * 3);
    }
}