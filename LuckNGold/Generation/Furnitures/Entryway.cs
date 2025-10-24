using LuckNGold.World.Furnitures.Enums;

namespace LuckNGold.Generation.Furnitures;

/// <summary>
/// A form of entryway, like a door or a gate.
/// </summary>
abstract record Entryway : Furniture
{
    public DoorOrientation Orientation { get; init; }

    public Entryway(Point position, Direction direction) : base(position)
    {
        // Establish orientation of the door
        if (direction.IsHorizontal())
        {
            Orientation = direction == Direction.Left ?
                DoorOrientation.Left : DoorOrientation.Right;
        }
        else
        {
            Orientation = direction == Direction.Up ?
                DoorOrientation.TopLeft : DoorOrientation.BottomLeft;
        }
    }
}