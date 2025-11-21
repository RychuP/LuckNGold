using LuckNGold.Primitives;
using LuckNGold.Resources;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

internal class CollectableFactory
{
    public static AnimatedRogueLikeEntity Coin()
    {
        var coin = new AnimatedRogueLikeEntity("Coin", false, GameMap.Layer.Items)
        {
            Name = "Coin"
        };
        coin.AllComponents.Add(new CurrencyComponent(1));
        return coin;
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
        var gemstone = ItemFactory.GetEntity(name, description: description);
        gemstone.AllComponents.Add(new ValueComponent(value));
        return gemstone;
    }
}