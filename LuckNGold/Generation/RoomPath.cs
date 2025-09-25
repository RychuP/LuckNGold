namespace LuckNGold.Generation;

internal class RoomPath(string name, RoomPath? parent = null, Room? startRoom = null)
{
    /// <summary>
    /// Name of the path.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// List of rooms in the path.
    /// </summary>
    public List<Room> Rooms { get; } = [];

    /// <summary>
    /// Parent path from which this path originates.
    /// </summary>
    public RoomPath? Parent { get; } = parent;

    /// <summary>
    /// Room from the parent path which forms the start of this path.
    /// </summary>
    public Room? StartRoom { get; } = startRoom;

    public Room FirstRoom => Rooms.First();
    public Room LastRoom => Rooms.Last();
    public int Count => Rooms.Count;
    public void Clear() => Rooms.Clear();
    public void Add(Room room) => Rooms.Add(room);
    public bool Remove(Room room) => Rooms.Remove(room);
}