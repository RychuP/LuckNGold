using LuckNGold.World.Items.Enums;
using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be worn like an armor or clothing.
/// </summary>
internal interface IWearable : ICarryable
{
    BodyPart BodyPart { get; }
    ClothingLayer Layer { get; }
}