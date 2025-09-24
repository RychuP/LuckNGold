namespace LuckNGold.Generation;

internal class DeadEnd(Direction direction) : IWallConnection
{
    public Direction Direction { get; } = direction;
}