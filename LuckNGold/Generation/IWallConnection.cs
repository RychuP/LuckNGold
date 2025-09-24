namespace LuckNGold.Generation;


internal interface IWallConnection
{
    /// <summary>
    /// Direction of the wall connection from the center of the room
    /// </summary>
    Direction Direction { get; }
}