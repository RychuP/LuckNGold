using LuckNGold.World.Common.Enums;

namespace LuckNGold.Generation.Decors;

record Candle : Decor
{
    public Size Size { get; init; }
    public Candle(Point position, Size size) : base(position)
    {
        Size = size;
    }
}