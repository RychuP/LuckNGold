namespace LuckNGold.Generation.Furnitures;

record Door : Entryway
{
    public bool IsDouble { get; init; }
    public Lock? Lock { get; init; }

    /// <summary>
    /// Initializes an instance of <see cref="Door"/> record.
    /// </summary>
    /// <param name="position">Position of the <see cref="Door"/>.</param>
    /// <param name="direction">Direction the door is facing.</param>
    /// <param name="isDouble">Whether <see cref="Door"/> occupies a single tile 
    /// or two tiles, in which case another door needs to be created and bound together.</param>
    /// <param name="lock">Object representing a padlock placed on <see cref="Door"/>.</param>
    public Door (Point position, Direction direction, bool isDouble = false, 
        Lock? @lock = null) : base(position, direction)
    {
        IsDouble = isDouble;
        Lock = @lock;
    }
}