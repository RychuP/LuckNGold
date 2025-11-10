using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates empty left hand, shield near - layer 9.
    /// </summary>
    void UpdateLeftHandLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var identityComponent = Parent.AllComponents.GetFirst<IdentityComponent>();
        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();

        Race race = identityComponent.Race;
        string raceType = race.RaceType.ToString().ToLower();
        string skinTone = race.SkinTone switch
        {
            SkinTone.Pale => "-pale",
            SkinTone.Dark => "-dark",
            _ => string.Empty,
        };

        string fontName = string.Empty;
        int row = 0, column = 0;

        // Weapon in right hand.
        if (equipment.RightHand != null)
        {

        }

        // Empty right hand.
        else
        {
            if (race == Race.Human)
            {
                fontName = $"race-{raceType}-base{skinTone}";
                row = 17;
                column = 0;
            }
        }

        SetLayerAppearance(OnionLayerName.LeftHand, fontName, row * 4, column * 3);
    }
}