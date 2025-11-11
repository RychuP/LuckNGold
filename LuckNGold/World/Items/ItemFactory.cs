using GoRogue.Random;
using LuckNGold.Primitives;
using LuckNGold.Resources;
using LuckNGold.World.Common.Components;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

/// <summary>
/// Factory of loot, tools, weapons and other collectibles.
/// </summary>
static class ItemFactory
{
    public static AnimatedRogueLikeEntity Key(Gemstone gemstone)
    {
        var animation = $"{gemstone}Key";
        var key = new AnimatedRogueLikeEntity(animation, true, GameMap.Layer.Items)
        {
            Name = $"{gemstone} Key"
        };
        key.AllComponents.Add(new UnlockingComponent((Quality)gemstone));
        return key;
    }

    public static AnimatedRogueLikeEntity Coin()
    {
        var coin = new AnimatedRogueLikeEntity("Coin", false, GameMap.Layer.Items)
        {
            Name = "Coin"
        };
        coin.AllComponents.Add(new CurrencyComponent(1));
        return coin;
    }

    public static RogueLikeEntity PeasantClothing()
    {
        var clothing = GetEntity(Strings.PeasantClothingName, Strings.PeasantClothingDescription);
        clothing.AllComponents.Add(new EquippableComponent(EquipSlot.Body));
        return clothing;
    }

    public static RogueLikeEntity Onyx() =>
        Gemstone("Onyx", 2, Strings.OnyxGemstoneDescription);

    public static RogueLikeEntity Amber() =>
        Gemstone("Amber", 4, Strings.AmberGemstoneDescription);

    public static RogueLikeEntity Emerald() =>
        Gemstone("Emerald", 8, Strings.EmeraldGemstoneDescription);

    public static RogueLikeEntity Ruby() =>
        Gemstone("Ruby", 16, Strings.RubyGemstoneDescription);

    public static RogueLikeEntity Diamond() =>
        Gemstone("Diamond", 32, Strings.DiamondGemstoneDescription);

    static RogueLikeEntity Gemstone(string name, int value, string description)
    {
        var gemstone = GetEntity(name, description: description);
        gemstone.AllComponents.Add(new ValueComponent(value));
        return gemstone;
    }

    static AnimatedRogueLikeEntity GetAnimatedEntity(string animationName, string name = "",
        string description = "", bool animationIsReversable = false)
    {
        var entity = new AnimatedRogueLikeEntity(animationName, animationIsReversable,
            GameMap.Layer.Items)
        { Name = name != "" ? name : animationName };
        AddDescriptionComponent(entity, description);
        return entity;
    }

    public static RogueLikeEntity GetEntity(string name = "", string description = "")
    {
        var glyphDef = Program.Font.GetGlyphDefinition(name);
        var appearance = glyphDef.CreateColoredGlyph();
        var entity = new RogueLikeEntity(appearance, layer: (int)GameMap.Layer.Items)
        {
            Name = name
        };
        AddDescriptionComponent(entity, description);
        return entity;
    }

    static void AddDescriptionComponent(RogueLikeEntity entity, string description)
    {
        if (!string.IsNullOrEmpty(description))
        {
            var descriptionComponent = new DescriptionComponent(description);
            entity.AllComponents.Add(descriptionComponent);
        }
    }
}