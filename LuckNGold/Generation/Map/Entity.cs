namespace LuckNGold.Generation.Map;

record Entity
{
    public Point Position { get; init; }

    public Entity(Point position)
    {
        Position = position;
    }
}