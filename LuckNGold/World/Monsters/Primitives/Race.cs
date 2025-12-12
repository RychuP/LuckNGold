using LuckNGold.Config;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives.Interfaces;

namespace LuckNGold.World.Monsters.Primitives;

record Race : IRace
{
    // Properties.
    required public RaceType RaceType { get; init; }
    public SkinTone SkinTone { get; init; } = SkinTone.None;
    public bool CanGrowHair { get; init; } = false;
    public bool CanGrowBeard { get; init; } = false;
    public bool CanChangeEyeColor { get; init; } = false;
    public bool CanGlow { get; init; } = false;
    public bool HasFlying { get; init; } = false;
    public int BaseMoveCost { get; init; } = GameSettings.TurnTime;

    // Predefined types.
    public static Race Human { get; }
    public static Race DarkHuman { get; }
    public static Race Elf { get; }
    public static Race DarkElf { get; }
    public static Race Skeleton { get; }
    public static Race Ogre { get; }

    // Private constructor.
    private Race() { }

    // Static constructor to initialize predefined types.
    static Race()
    {
        Human = new Race()
        {
            RaceType = RaceType.Human,
            CanGrowHair = true,
            CanGrowBeard = true,
            SkinTone = SkinTone.Pale,
            BaseMoveCost = GameSettings.TurnTime - 5,
        };

        DarkHuman = Human with
        {
            SkinTone = SkinTone.Dark,
        };

        Elf = new Race()
        {
            RaceType = RaceType.Elf,
            SkinTone = SkinTone.Pale,
            CanGrowHair = true,
            BaseMoveCost = GameSettings.TurnTime - 20,
        };

        DarkElf = Elf with
        {
            SkinTone = SkinTone.Dark,
        };

        Skeleton = new Race()
        {
            RaceType = RaceType.Skeleton,
            CanGlow = true,
            BaseMoveCost = GameSettings.TurnTime + 20,
        };

        Ogre = new Race()
        {
            RaceType = RaceType.Ogre,
            BaseMoveCost = GameSettings.TurnTime + 10,
        };
    }
}