using LuckNGold.World.Common.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Common.Components;

internal class DescriptionComponent(string description, string stateDescription = "") : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IDescription
{
    public string Description { get; init; } = description;
    public string StateDescription { get; set; } = stateDescription;
}