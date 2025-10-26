using SadConsole.Entities;

namespace LuckNGold.Generation.Map;

/// <summary>
/// Simple, square, walled space with four available exits placed centrally on each wall.
/// Basic building block of the dungeon map.
/// </summary>
partial class Room
{
    /// <summary>
    /// Number of rooms between this room and the exit room of the section.
    /// </summary>
    public int DistanceToSectionExit { get; set; }

    /// <summary>
    /// <see cref="RoomPath"/> that this room belongs to.
    /// </summary>
    public RoomPath Path { get; private set; }

    Section? _section = null;
    /// <summary>
    /// Section of the dungeon this room is part of.
    /// </summary>
    public Section? Section
    {
        get => _section;
        set
        {
            if (_section != null)
                throw new InvalidOperationException("Section should not change once allocated.");
            _section = value;
        }
    }

    /// <summary>
    /// Initializes and instance of the <see cref="Room"/> class with the given parameters.
    /// </summary>
    /// <param name="x">X coordinate of the position of the room on the map.</param>
    /// <param name="y">Y coordinate of the position of the room on the map.</param>
    /// <param name="width">Width of the room.</param>
    /// <param name="height">Height of the room.</param>
    /// <param name="parent">Path this room belongs to.</param>
    public Room(int x, int y, int width, int height, RoomPath parent)
    {
        Area = new Rectangle(x, y, width, height);
        Bounds = Area.Expand(1, 1);
        Path = parent;
        CornerPositions = GetCornerPositions();
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Room"/> class with the given parameters.
    /// </summary>
    /// <param name="position">Position on the map.</param>
    /// <param name="width">Width of the room.</param>
    /// <param name="height">Height of the room.</param>
    /// <param name="parent">Path this room belongs to.</param>
    public Room(Point position, int width, int height, RoomPath parent) :
        this(position.X, position.Y, width, height, parent) { }

    /// <summary>
    /// Transfers <see cref="Room"/> from one <see cref="RoomPath"/> to another.
    /// </summary>
    /// <param name="newPath"><see cref="RoomPath"/> that the room will be transfered to.</param>
    /// <returns>True if transfer succeeded, false otherwise.</returns>
    public bool TransferToPath(RoomPath newPath)
    {
        if (newPath == Path)
            return true;

        if (newPath.Count == 0)
            newPath.Add(this);
        
        else if (newPath.LastRoom.IsConnectedTo(this))
        {
            Path.Remove(this);
            newPath.Add(this);
            Path = newPath;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if this is the first room of the main path.
    /// </summary>
    public bool IsLevelEntrance =>
        Path.Name == "MainPath" && Path.FirstRoom == this;

    /// <summary>
    /// Checks if this is the last room of the main path.
    /// </summary>
    public bool IsLevelExit =>
        Path.Name == "MainPath" && Path.LastRoom == this;
}