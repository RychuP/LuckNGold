using LuckNGold.World.Common.Enums;

namespace LuckNGold.Generation.Decors;

record CandleStand : Decor
{
    public Size Size { get; init; }
    public CandleStand(Point position, Size size) : base(position)
    {
        Size = size;
    }
}