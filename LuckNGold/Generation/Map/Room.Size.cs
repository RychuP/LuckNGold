using GoRogue.Random;

namespace LuckNGold.Generation.Map;

// Properties and methods relating to the room size generation and retrieval.
partial class Room
{
    /// <summary>
    /// Min odd wall size.
    /// </summary>
    /// <remarks>Cannot be set to less than 3.</remarks>
    public const int MinOddSize = 3;

    /// <summary>
    /// Max odd wall size.
    /// </summary>
    public const int MaxOddSize = 9;

    /// <summary>
    /// Min even size of the horizontal wall.
    /// Vertical walls are always odd.
    /// </summary>
    public const int MinEvenSize = MinOddSize + 1;

    /// <summary>
    /// Max even size of the horizontal wall.
    /// Vertical walls are always odd.
    /// </summary>
    public const int MaxEvenSize = MaxOddSize + 1;

    /// <summary>
    /// Minimum ratio of shorter length to longer length. 
    /// </summary>
    public const double MinSizeRatio = 0.65d;

    /// <summary>
    /// Returns the size of the wall in the given direction from the center.
    /// </summary>
    public int GetWallSize(Direction direction) =>
        direction.IsHorizontal() ? Height : Width;

    public static int GetRandomOddSize(int min = MinOddSize, int max = MaxOddSize)
    {
        int size;
        do { size = GlobalRandom.DefaultRNG.NextInt(min, max + 1); }
        while (size.IsEven());
        return size;
    }

    public static int GetRandomEvenSize(int min = MinEvenSize, int max = MaxEvenSize)
    {
        int size;
        do { size = GlobalRandom.DefaultRNG.NextInt(min, max + 1); }
        while (size.IsOdd());
        return size;
    }

    public static int GetRandomSize(int min = MinOddSize, int max = MaxEvenSize) =>
        GlobalRandom.DefaultRNG.NextInt(min, max + 1);
}