using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    /// <summary>
    /// Updates hair, helmet - layer 6.
    /// </summary>
    void UpdateHeadwearLayer()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var identityComponent = Parent.AllComponents.GetFirst<IdentityComponent>();

        // Check if the race can grow hair at all.
        Race race = (Race)identityComponent.Race;
        if (!race.CanGrowHair) return;

        var equipment = Parent.AllComponents.GetFirst<EquipmentComponent>();
        var appearance = identityComponent.Appearance;

        
        string fontName = string.Empty;
        int row = 0, column = 0;

        // Helmet on the head.
        if (equipment.Head != null)
        {

        }

        // Hair only. Applies to humans and elves. All other races don't grow hair.
        else
        {
            // Shaved or bald head don't need hair.
            if (appearance.HairStyle == HairStyle.Bald ||
                appearance.HairStyle == HairStyle.Shaved)
            {
                EraseLayer(OnionLayerName.Headwear);
                return;
            }

            string raceType = race.RaceType.ToString().ToLower();
            string skinTone = race.RaceType == RaceType.Elf && race.SkinTone == SkinTone.Dark ?
                "dark-" : string.Empty;
            var hairVariant = (int)appearance.HairColor + 1;
            fontName = $"race-{skinTone}{raceType}-hair-{hairVariant}";

            column = (int)appearance.HairCut;
            row = appearance.HairStyle switch
            {
                HairStyle.Long => 1,
                _ => 0
            };
        }

        SetLayerAppearance(OnionLayerName.Headwear, fontName, row * 4, column * 3);
    }
}