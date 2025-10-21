namespace LuckNGold.Generation.Decors;

record FountainBottom : Decor
{
    public string Color { get; init; }
    public FountainBottom(Point position, bool isBlue = true) : base(position)
    {
        Color = isBlue ? "Blue" : "Red";
    }
}