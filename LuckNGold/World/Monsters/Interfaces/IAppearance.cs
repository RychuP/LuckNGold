using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Interfaces;

internal interface IAppearance
{
    HairStyle HairStyle { get; }
    HairCut HairCut { get; }
    BeardStyle BeardStyle { get; }
}