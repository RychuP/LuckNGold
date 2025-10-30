using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Furnitures;

record Door : Entryway
{
    public Lock? Lock { get; init; }

    /// <summary>
    /// Initializes an instance of <see cref="Door"/> record.
    /// </summary>
    public Door (Exit exit, Lock? @lock = null) : base(exit)
    {
        Lock = @lock;
    }
}