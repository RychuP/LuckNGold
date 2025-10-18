namespace LuckNGold.Generation.Map;


internal interface IWallConnection
{
    /// <summary>
    /// Direction to the wall connection from the center of the room.
    /// </summary>
    Direction Direction { get; }
}