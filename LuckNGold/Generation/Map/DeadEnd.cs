namespace LuckNGold.Generation.Map;

internal class DeadEnd(Direction direction) : IWallConnection
{
    public Direction Direction { get; } = direction;
}