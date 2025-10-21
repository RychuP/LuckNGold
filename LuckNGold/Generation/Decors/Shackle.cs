namespace LuckNGold.Generation.Decors;

record Shackle : Decor
{
    public string Size { get; init; }
    public Shackle(Point position, string size) : base(position)
    {
        Size = size;
    }
}