using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates shield far - layer 1.
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

    void DrawShieldFar()
    {

    }
}