using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates base appearance - layer 2.
    /// </summary>
    void UpdateBaseLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var identityComponent = Parent.AllComponents.GetFirst<IdentityComponent>();
        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();

        Race race = (Race)identityComponent.Race;
        var appearance = identityComponent.Appearance;

        string raceType = race.RaceType.ToString().ToLower();
        string skinTone = race.SkinTone switch
        {
            SkinTone.Pale => "-pale",
            SkinTone.Dark => "-dark",
            _ => string.Empty,
        };

        string fontName = $"race-{raceType}-base{skinTone}";
        int row = 0, column = 0;

        // Wearing a hood or a helmet. Hair not visible.
        if (equipment.Head != null)
        {

        }

        // Hair, bald or shaved head.
        else
        {
            if (race == Race.Human)
            {
                column = appearance.HairStyle switch
                {
                    HairStyle.Bald => 0,
                    HairStyle.Shaved => 1,
                    _ => 2,
                };

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
        }

        SetLayerAppearance(OnionLayerName.Base, fontName, row * 4, column * 3);
    }
}