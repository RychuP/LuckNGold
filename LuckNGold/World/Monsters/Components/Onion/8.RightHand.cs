using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates empty right hand, weapon hand - layer 8.
    /// </summary>
    void UpdateRightHandLayer()
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
        if (equipment.RightHand is RogueLikeEntity weapon)
        {
            if (race == Race.Human)
            {
                fontName = $"race-{raceType}-weapon-hands{skinTone}";
            }

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

        // Empty right hand.
        else
        {
            if (race == Race.Human)
            {
                fontName = $"race-{raceType}-base{skinTone}";
                row = 17;
                column = 2;
            }
        }

        SetLayerAppearance(OnionLayerName.RightHand, fontName, row * 4, column * 3);
    }
}