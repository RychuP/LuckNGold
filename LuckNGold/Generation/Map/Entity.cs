namespace LuckNGold.Generation.Map;

abstract record Entity
{
    public Point Position { get; init; }

    public Entity(Point position)
    {
        Position = position;
    }
}