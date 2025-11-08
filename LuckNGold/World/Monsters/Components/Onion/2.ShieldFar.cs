using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates shield far (2) layer.
    /// </summary>
    void UpdateShieldFarLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Wielding a shield in the left hand.
        if (equipment.LeftHand != null)
        {

        }

        // Left hand empty.
        else
        {
            EraseLayer(OnionLayerName.ShieldFar);
            return;
        }

        SetLayerAppearance(OnionLayerName.ShieldFar, fontName, row * 4, column * 3);
    }
}