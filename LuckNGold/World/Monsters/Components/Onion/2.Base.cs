using LuckNGold.Resources;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates base appearance - layer 2.
    /// </summary>
    void UpdateBaseLayer(RogueLikeEntity? headwear = null)
    {
        Race race = IdentityComponent.Race;
        var appearance = IdentityComponent.Appearance;

        string raceType = GetRaceTypeText(race);
        string skinTone = GetSkinToneText(race);
        string fontName = $"race-{raceType}-base{skinTone}";
        int row = 0, col = 0;

        // Wearing a hood or a helmet. Hair not visible.
        if (headwear != null)
        {
            if (race == Race.Human)
            {
                col = headwear.Name.Contains(Strings.HelmetTag) ? 4 : 3;
            }
            else if (race == Race.Skeleton)
            {
                col = headwear.Name.Contains(Strings.HelmetTag) ? 3 : 2;
            }
        }

        // Hair, bald or shaved head.
        else
        {
            // Humans can be bald, shaved, grow hair or beards.
            if (race == Race.Human)
            {
                // Crew cut should not use the appearance with hair (col 2).
                if (appearance.HairStyle == HairStyle.Short && appearance.HairCut == HairCut.VariantD)
                {
                    // It should be either bald or shaved... I will go with bald.
                    col = 0;
                }

                // Any other hair combo.
                else
                {
                    col = appearance.HairStyle switch
                    {
                        HairStyle.Bald => 0,
                        HairStyle.Shaved => 1,
                        _ => 2,
                    };
                }
            }

            // Skeletons don't have hair.
            else if (race == Race.Skeleton)
            {
                // Skeletons have a separate column for clothed appearance for some reason...
                RogueLikeEntity? bodywear = EquipmentComponent.Body;
                col = bodywear is null ? 0 : 1;
            }
        }

        // Humans have a variety of age and facial expressions.
        if (race == Race.Human)
        {
            row = appearance switch
            {
                { Age: Age.Young, Face: Face.VariantA, IsAngry: false } => 0,
                { Age: Age.Young, IsAngry: true } => 2,

                { Age: Age.Adult, BeardStyle: BeardStyle.Circle, IsAngry: false } => 11,
                { Age: Age.Adult, BeardStyle: BeardStyle.Circle, IsAngry: true } => 12,

                { Age: Age.Adult, BeardStyle: BeardStyle.Boxed, IsAngry: false } => 13,
                { Age: Age.Adult, BeardStyle: BeardStyle.Boxed, IsAngry: true } => 14,

                { Age: Age.Old, Face: Face.VariantA, BeardStyle: BeardStyle.None, IsAngry: false } => 3,
                { Age: Age.Old, Face: Face.VariantB, BeardStyle: BeardStyle.None, IsAngry: false } => 5,
                { Age: Age.Old, Face: Face.VariantC, BeardStyle: BeardStyle.None, IsAngry: false } => 6,
                { Age: Age.Old, BeardStyle: BeardStyle.None, IsAngry: true } => 7,

                { Age: Age.Old, Face: Face.VariantA, BeardStyle: BeardStyle.Circle, IsAngry: false } => 4,
                { Age: Age.Old, Face: Face.VariantB, BeardStyle: BeardStyle.Circle, IsAngry: false } => 8,
                { Age: Age.Old, Face: Face.VariantC, BeardStyle: BeardStyle.Circle, IsAngry: false } => 9,
                { Age: Age.Old, BeardStyle: BeardStyle.Circle, IsAngry: true } => 10,

                _ => 15 // young, variant b, not angry
            };
        }

        // Skeletons have a variety of glowing eyes. They have a full glow appearance.
        // They can also have a head only flying appearance.
        else if (race == Race.Skeleton)
        {
            row = 0;
        }

        SetLayerAppearance(OnionLayerName.Base, fontName, row * 4, col * 3);
    }
}