using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Primitives.Interfaces;

interface IRace
{
    RaceType RaceType { get; init; } 
    SkinTone SkinTone { get; }
    bool CanGrowHair { get; init; }
    bool CanGrowBeard { get; init; }
    bool CanChangeEyeColor { get; init; }
    bool CanGlow { get; init; }
    bool HasFlying { get; init; }
    int BaseMoveCost { get; init; }
}