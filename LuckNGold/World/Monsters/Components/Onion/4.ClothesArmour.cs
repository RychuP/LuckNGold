using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates clothes / armour (4) layer.
    /// </summary>
    void UpdateClothesArmourLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Wearing clothes / armour.
        if (equipment.Torso != null)
        {

        }

        // Not wearing anything.
        else
        {
            EraseLayer(OnionLayerName.ClothesArmour);
            return;
        }

        SetLayerAppearance(OnionLayerName.ClothesArmour, fontName, row * 4, column * 3);
    }
}