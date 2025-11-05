using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

namespace LuckNGold.World.Monsters.Primitives;

record struct Appearance(HairStyle HairStyle = default, HairCut HairCut = default,
    BeardStyle BeardStyle = default) : IAppearance;