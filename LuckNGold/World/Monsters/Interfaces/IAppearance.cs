using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Interfaces;

internal interface IAppearance
{
    Age Age { get; }
    Face Face { get; }
    EyeColor EyeColor { get; }
    HairStyle HairStyle { get; }
    HairCut HairCut { get; }
    HairColor HairColor { get; }
    BeardStyle BeardStyle { get; }
    BeardColor BeardColor { get; }
    bool IsAngry { get; }
}