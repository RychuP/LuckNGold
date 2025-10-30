using GoRogue.Random;
using LuckNGold.Primitives;
using LuckNGold.Resources;
using LuckNGold.Visuals;
using LuckNGold.World.Common.Components;
using LuckNGold.World.Common.Enums;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Decors;

static class DecorFactory
{
    /// <summary>
    /// Skull and bone that can be placed on the floor.
    /// </summary>
    public static RogueLikeEntity Skull(HorizontalOrientation orientation) =>
        GetEntity($"Skull{orientation}", name: "Bones",
            description: Strings.BonesDescription);

    /// <summary>
    /// Bones that can be placed on the floor.
    /// </summary>
    public static RogueLikeEntity Bones() =>
        GetEntity("Bones", randomMirror: true, description: Strings.BonesDescription);

    /// <summary>
    /// Boxes that can be placed on the floor.
    /// </summary>
    /// <param name="size">Size of the boxes: either large or small.</param>
    public static RogueLikeEntity Boxes(Size size)
    {
        string entityDescription = size == Size.Large ? Strings.CratesDescription :
            Strings.BoxesDescription;
        string entityName = size == Size.Large ? "Crates" : "Boxes";
        var entity = GetEntity(entityName, isWalkable: size == Size.Small, 
            randomMirror: true, description: entityDescription);
        return entity;
    }

    /// <summary>
    /// Spider web that can be placed on the floor in the corners of a room.
    /// </summary>
    /// <param name="orientation">Horizontal appearance of the web.</param>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static RogueLikeEntity SpiderWeb(Size size, HorizontalOrientation orientation) =>
        GetEntity($"FloorSpiderWeb{size}{orientation}", name: "Spider Web",
            description: Strings.SpiderWebDescription);

    public static RogueLikeEntity Shackle(string size) =>
        GetEntity($"Shackle{size}", name: "Shackles", 
            description: Strings.ShacklesDescription);

    /// <summary>
    /// Steps leading to upper or lower levels.
    /// </summary>
    public static RogueLikeEntity Steps(bool faceRight, bool leadDown)
    {
        string direction = leadDown ? "Down" : "Up";
        string face = faceRight ? "Right" : "Left";
        string stepsName = $"Steps {direction}";
        string stepsDescription = leadDown ? Strings.StepsDownDescription :
            Strings.StepsUpDescription;
        return GetEntity($"Steps{direction}{face}", name: stepsName, 
            description: stepsDescription);
    }

    public static RogueLikeEntity CandleStand(Size size) =>
        GetEntity($"CandleStand{size}", name: "Candle Stand", false,
            description: Strings.CandleStandDescription);

    public static RogueLikeEntity Urn() =>
        GetEntity("Urn", description: Strings.UrnDescription);

    public static AnimatedRogueLikeEntity Cauldron() =>
        GetAnimatedEntity("Cauldron", isWalkable: false, description: Strings.CauldronDescription);

    /// <summary>
    /// Animated torch that can be placed on the side wall of a room.
    /// </summary>
    /// <exception cref="ArgumentException">Fired when the wrong type 
    /// of direction is passed.</exception>
    public static AnimatedRogueLikeEntity SideTorch(HorizontalOrientation orientation) =>
        GetAnimatedEntity($"SideTorch{orientation}", "Torch", 
            description: Strings.TorchDescription);

    /// <summary>
    /// Animated torch that can be placed on the top wall of a room.
    /// </summary>
    public static AnimatedRogueLikeEntity Torch() =>
        GetAnimatedEntity("Torch", description: Strings.TorchDescription);

    /// <summary>
    /// Free standing, burning candle on a non walkable stand that can be placed on the floor.
    /// </summary>
    /// <param name="size">Size of the stand.</param>
    public static AnimatedRogueLikeEntity Candle(Size size) =>
        GetAnimatedEntity($"{size}Candle", name: "Candle", isWalkable: false,
            description: Strings.CandleDescription);

    /// <summary>
    /// Animated banner that can be placed on the top wall of a room.
    /// </summary>
    /// <param name="color">Color of the inner portion of the banner that corresponds 
    /// to the given <see cref="Gemstone"/>.</param>
    public static AnimatedRogueLikeEntity Banner(Gemstone color)
    {
        string colorDescription = color switch
        {
            Gemstone.Onyx => Strings.OnyxBannerDescription,
            Gemstone.Amber => Strings.AmberBannerDescription,
            Gemstone.Emerald => Strings.EmeraldBannerDescription,
            Gemstone.Ruby => Strings.RubyBannerDescription,
            Gemstone.Diamond => Strings.DiamondBannerDescription,
            _ => string.Empty
        };

        return GetAnimatedEntity($"{color}Banner", name: $"{color} Banner",
            description: Strings.BannerDescription, stateDescription: colorDescription);
    }

    public static AnimatedRogueLikeEntity Fountain(VerticalOrientation orientation, bool isBlue)
    {
        var color = isBlue ? "Blue" : "Red";
        string colorDescription = isBlue ? Strings.BlueFountainDescription :
            Strings.RedFountainDescription;
        bool isWalkable = orientation == VerticalOrientation.Top;
        return GetAnimatedEntity($"{color}Fountain{orientation}", name: "Fountain",
            isWalkable: isWalkable, description: Strings.FountainDescription,
            stateDescription: colorDescription);
    }

    static AnimatedRogueLikeEntity GetAnimatedEntity(string animationName, string name = "",
        bool isWalkable = true, string description = "", string stateDescription = "")
    {
        var entity = new AnimatedRogueLikeEntity(animationName, false, 
            GameMap.Layer.Decor, isWalkable) 
            { Name = name != "" ? name : animationName};
        AddDescriptionComponent(entity, description, stateDescription);
        return entity;
    }

    static RogueLikeEntity GetEntity(string definitionName, string name = "",
        bool isWalkable = true, bool randomMirror = false,
        string description = "", string stateDescription = "")
    {
        var glyphDef = Program.Font.GetGlyphDefinition(definitionName);
        var appearance = glyphDef.CreateColoredGlyph();
        var entity = new RogueLikeEntity(appearance, layer: (int)GameMap.Layer.Decor)
        {
            Name = !string.IsNullOrEmpty(name) ? name : definitionName,
            IsWalkable = isWalkable
        };
        if (randomMirror)
        {
            var mirror = GlobalRandom.DefaultRNG.NextBool() ? 0 : 2;
            entity.AppearanceSingle!.Appearance.Mirror = (Mirror)mirror;
        }
        AddDescriptionComponent(entity, description, stateDescription);
        return entity;
    }

    static void AddDescriptionComponent(RogueLikeEntity entity, string description,
        string stateDescription)
    {
        if (!string.IsNullOrEmpty(description))
        {
            var descriptionComponent = new DescriptionComponent(description, stateDescription);
            entity.AllComponents.Add(descriptionComponent);
        }
    }
} 