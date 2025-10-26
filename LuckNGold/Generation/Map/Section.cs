using LuckNGold.World.Items.Enums;
using static System.Collections.Specialized.BitVector32;

namespace LuckNGold.Generation.Map;

/// <summary>
/// Part of the dungeon between two progressively more difficult locked doors.</remarks>
/// </summary>
internal class Section(Gemstone gemstone)
{
    /// <summary>
    /// Gemstone of the section.
    /// </summary>
    /// <remarks>Lower index of the gemstone means easier difficulty of the section.</remarks>
    public Gemstone Gemstone => gemstone;

    /// <summary>
    /// Room of the main path that is the first room of the section.
    /// </summary>
    public Room Entrance => Rooms[0];

    Room? _exit = default;
    /// <summary>
    /// Room of the main path that is the last room of the section
    /// and holds the locked door to the next section.
    /// </summary>
    public Room? Exit
    {
        get => _exit;
        set
        {
            if (value is not null && !_rooms.Contains(value))
                throw new InvalidOperationException("Exit room has to be part of the sction.");
            _exit = value;
        }
    }

    readonly List<Room> _rooms = [];
    /// <summary>
    /// List of all the rooms in the section.
    /// </summary>
    public IReadOnlyList<Room> Rooms => _rooms;


    List<Room> _singleRooms = [];
    /// <summary>
    /// All rooms with a single entrance ordered by distance to section exit.
    /// </summary>
    /// <remarks>Dungeon entrance room is removed from that list.</remarks>
    public IReadOnlyList<Room> SingleRooms
    {
        get
        {
            if (_singleRooms.Count == 0)
            {
                _singleRooms = GetSingleRooms();
                if (IsFirst())
                    _singleRooms.Remove(Entrance);
            }
            return _singleRooms;
        }
    }

    /// <summary>
    /// Adds rooms from the main path and all other rooms from side paths connected to it.
    /// </summary>
    /// <param name="room"><see cref="Room"/> from the main path.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(Room room)
    {
        if (room.Path.Name != "MainPath")
            throw new InvalidOperationException("Only main path rooms can be a parameter " +
                "of this method.");

        // Add room to the section.
        _rooms.Add(room);
        room.Section = this;

        // Check room has any side paths.
        if (!room.IsPathStartRoom())
            return;

        // Prepare a queue of paths connected to the room from the main path.
        Queue<RoomPath> pathsToBeAdded = [];
        AddPathsToQueue(room.SidePaths);

        while (pathsToBeAdded.Count > 0)
        {
            var path = pathsToBeAdded.Dequeue(); ;

            foreach (var sideRoom in path.Rooms)
            {
                _rooms.Add(sideRoom);
                sideRoom.Section = this;
                if (sideRoom.IsPathStartRoom())
                    AddPathsToQueue(sideRoom.SidePaths);
            }
        }

        void AddPathsToQueue(IEnumerable<RoomPath> paths)
        {
            foreach (var path in paths)
                pathsToBeAdded.Enqueue(path);
        }
    }

    /// <summary>
    /// Gets all rooms with a single entrance ordered by distance to section exit.
    /// </summary>
    List<Room> GetSingleRooms() => [.. Rooms
        .Where(r => r.Exits.Count() == 1)
        .OrderBy(r => r.DistanceToSectionExit)];

    /// <summary>
    /// Gets distance to the room either from <see cref="Entrance"/> 
    /// or <see cref="Exit"/> measured in number of rooms.
    /// </summary>
    /// <param name="room"><see cref="Room"/> to find the distance to.</param>
    /// <param name="fromExit">True if <see cref="Exit"/> is the start of the distance 
    /// or false it <see cref="Entrance"/> is the start.</param>
    /// <returns>Distance to the room 
    /// or -1 if <see cref="Room"/> is not part of the section.</returns>
    public int GetDistance(Room room, bool fromExit = true)
    {
        if (!_rooms.Contains(room))
            return -1;

        var startRoom = (fromExit ? Exit : Entrance) ?? 
            throw new InvalidOperationException("Start room should not be null.");

        var path = room.Path;
        int count = 0;
        while (path != startRoom.Path)
        {
            int dist = path.GetDistance(room, path.FirstRoom);
            if (dist == -1)
                throw new InvalidOperationException("Room is not part of the path.");
            count += dist + 1;

            if (path.StartRoom is null)
                throw new InvalidOperationException("All paths encountered at this stage " +
                    "of the search should have start rooms set.");

            room = path.StartRoom;
            path = room.Path;
        }
        
        return count + path.GetDistance(room, startRoom);
    }

    /// <summary>
    /// Whether the <see cref="Section"/> is the first in the dungeon.
    /// </summary>
    /// <returns></returns>
    public bool IsFirst() =>
        Gemstone == Gemstone.Onyx;
}