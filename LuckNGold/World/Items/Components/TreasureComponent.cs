using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an entity that has monetary value and can be considered a treasure.
/// </summary>
/// <param name="type">Type of <see cref="ITreasure"/> that this component represents.</param>
/// <param name="value">Monetary value of the entity with this component.</param>
class TreasureComponent(TreasureType type, int value) : 
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ITreasure
{
    public int Value { get; } = value;
    public TreasureType Type { get; } = type;
}