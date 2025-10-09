using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using LuckNGold.World.Monsters.Enums;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can be worn like armor or clothing.
/// </summary>
/// <param name="bodyPart">Body part where this item can be worn.</param>
internal class WearableComponent(BodyPart bodyPart, ClothingLayer layer) : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IWearable
{
    public BodyPart BodyPart => bodyPart;
    public ClothingLayer Layer => layer;
}