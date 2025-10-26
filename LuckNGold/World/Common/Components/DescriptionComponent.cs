using LuckNGold.World.Common.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Common.Components;

internal class DescriptionComponent : RogueLikeComponentBase<RogueLikeEntity>, IDescription
{
    public string Description { get; init; }

    public DescriptionComponent(string description) : base(false, false, false, false)
    {
        Description = description;
    }
}