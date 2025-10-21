namespace LuckNGold.Generation.Decors;

record FountainTop : Decor
{
    public string Color { get; init; }
    public FountainTop(Point position, bool isBlue = true) : base(position)
    {
        Color = isBlue ? "Blue" : "Red";
    }
}