namespace LuckNGold;

/// <summary>
/// Raised when a problem arises with establishing a new connection to a room.
/// </summary>
class RoomConnectionException(string msg) : Exception(msg)
{ }

/// <summary>
/// Raised when a probe area gets too small meaning there is no sufficient space for a new room.
/// </summary>
class ProbeException(string msg) : Exception(msg)
{ }