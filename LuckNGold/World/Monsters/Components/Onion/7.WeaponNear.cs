using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates weapon near - layer 7.
    /// </summary>
    void UpdateWeaponNearLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Wielding a weapon in the right hand.
        if (equipment.RightHand is RogueLikeEntity weapon)
        {
            var materialComponent = weapon.AllComponents.GetFirst<IMaterial>();
            var material = materialComponent.Material;
            fontName = "weapons-1-near";

            if (weapon.AllComponents.Contains<IMeleeAttack>())
            {
                if (weapon.Name.Contains("Sword"))
                {
                    if (weapon.Name.Contains("Arming"))
                    {
                        row = 0;
                        column = 0;
                    }
                }
            }
        }

        // Right hand empty.
        else
        {
            EraseLayer(OnionLayerName.WeaponNear);
            return;
        }

        SetLayerAppearance(OnionLayerName.WeaponNear, fontName, row * 4, column * 3);
    }
}