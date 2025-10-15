using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation;

/// <summary>
/// Path made out of rooms connected one after another.
/// </summary>
/// <param name="name">Name of the path. Usually the same as the generation step
/// where the path was made.</param>
/// <param name="parent">Parent path where this path originates from. 
/// Only main path has no parent.</param>
/// <param name="startRoom">Room belonging to the parent path which forms the first room
/// of this path.</param>
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

    int _sideRoomCount = 0;
    /// <summary>
    /// Number of rooms from sub paths connected to this path.
    /// </summary>
    public int SideRoomCount
    {
        get => _sideRoomCount;
        set
        {
            var prevCount = _sideRoomCount;
            _sideRoomCount = value;

            // Update parent if any
            if (Parent is not null)
                Parent.SideRoomCount += value - prevCount;
        }
    }

    public Room FirstRoom => Rooms.First();
    public Room LastRoom => Rooms.Last();

    /// <summary>
    /// Number of rooms in this path only.
    /// </summary>
    public int Count => Rooms.Count;
    public void Clear() => Rooms.Clear();
    public void Add(Room room) => Rooms.Add(room);
    public bool Remove(Room room) => Rooms.Remove(room);

    /// <summary>
    /// Gets the list of all paths branching out from this path.
    /// </summary>
    //public List<RoomPath> GetAllPaths()
    //{
    //    List<RoomPath> paths = [];
    //    foreach (var room in Rooms)
    //        paths.AddRange(room.GetAllPaths());
    //    return paths;
    //}
}