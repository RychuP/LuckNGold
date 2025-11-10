using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates weapon far - layer 0.
    /// </summary>
    void UpdateWeaponFarLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Wielding a weapon in the right hand.
        if (equipment.RightHand != null)
        {

        }

        // Right hand empty.
        else
        {
            EraseLayer(OnionLayerName.WeaponFar);
            return;
        }

        SetLayerAppearance(OnionLayerName.WeaponFar, fontName, row * 4, column * 3);
    }
}