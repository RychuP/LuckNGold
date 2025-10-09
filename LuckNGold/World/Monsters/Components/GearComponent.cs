using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// It represents gear carried in hands of a monster entity.
/// </summary>
internal class GearComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IGear
{
    public Dictionary<Hand, RogueLikeEntity> ItemsWielded { get; } = [];

    /// <inheritdoc/>
    public bool Wield(RogueLikeEntity wieldable)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool PutAway(RogueLikeEntity wieldable)
    {
        throw new NotImplementedException();
    }

    public override void OnAdded(IScreenObject host)
    {
        base.OnAdded(host);

        if (Parent!.Layer != (int)GameMap.Layer.Monsters)
            throw new InvalidOperationException("Component is meant to be added " +
                "to a Monster entity.");
    }
}