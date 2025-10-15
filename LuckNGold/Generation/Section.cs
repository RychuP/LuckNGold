using LuckNGold.World.Items.Enums;
using System.Collections;

namespace LuckNGold.Generation;

/// <summary>
/// Part of the dungeon between two progressively more difficult locked doors.</remarks>
/// </summary>
internal class Section(Gemstone gemstone)
{
    /// <summary>
    /// Room of the main path that is the first room of the section.
    /// </summary>
    //public Room FirsRoom => firstRoom;

    /// <summary>
    /// Gemstone of the section.
    /// </summary>
    /// <remarks>Lower index of the gemstone means easier difficulty of the section.</remarks>
    public Gemstone Gemstone => gemstone;

    /// <summary>
    /// Room of the main path that is the last room of the section
    /// and holds the locked door to the next section.
    /// </summary>
    //public Room? LastRoom { get; set; }

    List<Room> _rooms = [];
    /// <summary>
    /// List of all the rooms in the section.
    /// </summary>
    public IReadOnlyList<Room> Rooms => _rooms;

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
        if (!room.IsStartRoom())
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
                if (sideRoom.IsStartRoom())
                    AddPathsToQueue(sideRoom.SidePaths);
            }
        }

        void AddPathsToQueue(IEnumerable<RoomPath> paths)
        {
            foreach (var path in paths)
                pathsToBeAdded.Enqueue(path);
        }
    }
}