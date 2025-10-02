using LuckNGold.World.Items.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It has intrinsic, monetary value.
/// </summary>
internal interface ITreasure : ICarryable
{
    int Value { get; }
    TreasureType Type { get; }
}