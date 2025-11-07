using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

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
        };

        DarkElf = Elf with
        {
            SkinTone = SkinTone.Dark,
        };

        Skeleton = new Race()
        {
            RaceType = RaceType.Skeleton,
            CanGlow = true,
        };

        Ogre = new Race()
        {
            RaceType = RaceType.Ogre,
        };
    }
}