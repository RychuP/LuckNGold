using LuckNGold.Primitives;
using LuckNGold.World.Common.Components;
using LuckNGold.World.Items.Components;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;

namespace LuckNGold.World.Items;

/// <summary>
/// Base item factory. Contains helper methods.
/// </summary>
static class ItemFactory
{
    static AnimatedRogueLikeEntity GetAnimatedEntity(string animationName, string name = "",
        string description = "", bool animationIsReversable = false)
    {
        var entity = new AnimatedRogueLikeEntity(animationName, animationIsReversable,
            GameMap.Layer.Items)
        { Name = name != "" ? name : animationName };
        AddDescriptionComponent(entity, description);
        return entity;
    }

    public static RogueLikeEntity GetEquippableEntity(string name, EquipSlot slot, 
        string description = "")
    {
        var entity = GetEntity(name, description);
        entity.AllComponents.Add(new EquippableComponent(slot));
        return entity;
    }

    public static RogueLikeEntity GetEntity(string name, string description = "")
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