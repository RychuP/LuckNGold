namespace LuckNGold;

/// <summary>
/// Raised when a problem arises with establishing a new connection to a room.
/// </summary>
class RoomConnectionException(string msg) : Exception(msg)
{ }