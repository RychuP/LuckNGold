using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Interfaces;

namespace LuckNGold.World.Monsters.Primitives;

record Race : IRace
{
    public static Race Human { get; }
    public static Race DarkHuman { get; }
    public static Race Elf { get; }
    public static Race DarkElf { get; } 
    public static Race Skeleton { get; }
    public static Race Ogre { get; }

    static Race()
    {
        Human = new Race()
        {
            RaceType = RaceType.Human,
            CanGrowHair = true,
            CanGrowBeard = true,
        };

        DarkHuman = Human with
        {
            IsDark = true,
        };

        Elf = new Race()
        {
            RaceType = RaceType.Elf,
            CanGrowHair = true,
        };

        DarkElf = Elf with
        {
            IsDark = true,
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

    private Race() { }

    required public RaceType RaceType { get; init; }
    public bool CanGrowHair { get; init; } = false;
    public bool CanGrowBeard { get; init; } = false;
    public bool IsDark { get; init; } = false;
    public bool HasFlying { get; init; } = false;
    public bool CanGlow { get; init; } = false;
}