using LuckNGold.Generation.Map;
using LuckNGold.World.Furnitures.Enums;
using System;

namespace LuckNGold.Generation.Furnitures;

/// <summary>
/// A form of entryway, like a door or a gate.
/// </summary>
abstract record Entryway : Furniture
{
    public DoorOrientation Orientation { get; init; }
    public bool IsDouble { get; init; }

    public Entryway(Exit exit) : base(exit.Position)
    {
        IsDouble = exit.IsDouble;

        // Establish orientation of the door
        if (exit.Direction.IsHorizontal())
        {
            Orientation = exit.Direction == Direction.Left ?
                DoorOrientation.Left : DoorOrientation.Right;
        }
        else
        {
            Orientation = exit.Direction == Direction.Up ?
                DoorOrientation.TopLeft : DoorOrientation.BottomLeft;
        }
    }
}