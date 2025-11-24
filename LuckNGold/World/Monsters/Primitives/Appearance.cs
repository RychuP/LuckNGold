using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives.Interfaces;

namespace LuckNGold.World.Monsters.Primitives;

record struct Appearance(
    Age Age = Age.Adult,
    Face Face = Face.VariantA,
    EyeColor EyeColor = EyeColor.Default,
    HairStyle HairStyle = default,
    HairCut HairCut = default,
    HairColor HairColor = default,
    BeardStyle BeardStyle = default,
    BeardColor BeardColor = default,
    bool IsAngry = false
) : IAppearance;