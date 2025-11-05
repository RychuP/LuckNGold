using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Interfaces;

interface IRace
{
    RaceType RaceType { get; init; } 
    bool CanGrowHair { get; init; }
    bool CanGrowBeard { get; init; }
    bool IsDark { get; init; }
    bool HasFlying { get; init; }
    bool CanGlow { get; init; }
}