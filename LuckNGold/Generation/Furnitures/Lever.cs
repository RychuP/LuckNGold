namespace LuckNGold.Generation.Furnitures;

record Lever : Furniture
{
    public Furniture Target { get; init; }

    public Lever(Point position, Furniture target) : base(position)
    {
        Target = target;
    }
}