using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.World.Items.Enums;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that creates objectives for the map by hiding section keys. 
/// </summary>
internal class ObjectiveGenerator() : GenerationStep("Objectives",
    new ComponentTypeTagPair(typeof(ItemList<Section>), "Sections"))
{
    readonly IEnhancedRandom _rnd = GlobalRandom.DefaultRNG;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var sections = context.GetFirst<ItemList<Section>>("Sections").Items;

        // Create and hide the keys to section doors.
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            if (i == 0)
                DoSimpleKeyPlacement(section);
            else
            {
                bool keyPlacementSucceded = _rnd.NextInt(2) switch
                {
                    0 => TryPlaceKeyInBarredRoom(section, sections),
                    _ => false
                };

                if (!keyPlacementSucceded)
                    DoSimpleKeyPlacement(section);
            }
        }

        yield break;
    }

    void DoSimpleKeyPlacement(Section section)
    {
        if (section.SingleRooms.Count >= 5)
            PlaceKeyInChests(section);
        else
            PlaceKeyInFarRoom(section);
    }

    /// <summary>
    /// Places the key in one of the further rooms in the section.
    /// </summary>
    void PlaceKeyInFarRoom(Section section)
    {
        Room room;
        var singleRoomCount = section.SingleRooms.Count;
        if (singleRoomCount > 1)
        {
            // Select a random room from a selection of distant, single rooms in the section.
            int startIndex = singleRoomCount / 2;
            int index = _rnd.NextInt(startIndex, singleRoomCount);
            room = section.SingleRooms[index];
        }
        else
        {
            // Select any room but first.
            int index = _rnd.NextInt(1, section.Rooms.Count);
            room = section.Rooms[index];
        }

        PlaceKeyInRoom(room, section.Gemstone);
    }

    void PlaceKeyInRoom(Room room, Gemstone gemstone)
    {
        // Find a random, free spot in the room not aligned with the room center.
        Point keyPosition;
        do keyPosition = _rnd.RandomPosition(room.Area);
        while (room.Contents.Where(e => e.Position == keyPosition).Any() ||
            keyPosition.X == room.Area.Center.X ||
            keyPosition.Y == room.Area.Center.Y);

        // Create the key.
        var key = new Key(keyPosition, gemstone);
        room.AddEntity(key);
    }

    /// <summary>
    /// Places a few chests in the section and the key in one of them.
    /// </summary>
    void PlaceKeyInChests(Section section)
    {
        int chestsNeeded = 3;
        if (section.SingleRooms.Count < chestsNeeded)
            chestsNeeded = section.SingleRooms.Count;
        int chestWithKeyIndex = _rnd.NextInt(chestsNeeded);
        var roomIndicesUsed = new List<int>(chestsNeeded);

        // Create chests.
        for (int i = 0; i < chestsNeeded; i++)
        {
            int roomIndex;
            do roomIndex = _rnd.RandomIndex(section.SingleRooms);
            while (roomIndicesUsed.Contains(roomIndex));
            roomIndicesUsed.Add(roomIndex);
            var room = section.SingleRooms[roomIndex];

            // Create current chest.
            var chestPosition = room.Area.Center;
            var chest = new Chest(chestPosition);

            // Put some coins in the chest.
            int coinsNeeded = _rnd.NextInt(5);
            for (int j = 0; j < coinsNeeded; j++)
            {
                var coin = new Coin(Point.None);
                chest.Items.Add(coin);
            }

            // Put key in the chest.
            if (chestWithKeyIndex == i)
            {
                var key = new Key(Point.None, section.Gemstone);
                chest.Items.Add(key);
            }

            room.AddEntity(chest);
        }
    }

    /// <summary>
    /// Places the <see cref="Key"/> in a single, barred room from a lower <see cref="Section"/>
    /// and a lever to open the barred gate in the current <see cref="Section"/>.
    /// </summary>
    /// <param name="section">Current <see cref="Section"/> where lever needs to be placed.</param>
    /// <param name="sections">List of all sections.</param>
    bool TryPlaceKeyInBarredRoom(Section section, IReadOnlyList<Section> sections)
    {
        if (section.IsFirst())
            return false;

        var lowerSection = sections.Where(s => s.Gemstone == section.Gemstone - 1).First();

        // Check section sizes are sufficient for the task.
        int smallSectionSize = 6;
        if (section.Rooms.Count <= smallSectionSize && lowerSection.Rooms.Count <= smallSectionSize)
        {
            // Try to go lower in sections but not too far.
            if (lowerSection.IsFirst()) 
                return false;
            lowerSection = sections.Where(s => s.Gemstone == section.Gemstone - 1).First();
            if (lowerSection.Rooms.Count <= smallSectionSize ) 
                return false;
        }

        // Check there is a free single room in the lower section.
        var freeSingleRooms = lowerSection.SingleRooms
            .Where(r => r.GetEntity<Furniture>() is null && r.GetEntity<Item>() is null)
            .ToArray();
        if (freeSingleRooms.Length < 1) return false;

        // Select a room for the key.
        int singleRoomIndex = _rnd.RandomIndex(freeSingleRooms);
        var room = freeSingleRooms[singleRoomIndex];
        PlaceKeyInRoom(room, section.Gemstone);

        // Place a gate at the room's exit.
        var exit = room.Exits.First();
        var gate = new Gate(exit);
        room.AddEntity(gate);

        // Select a room for the lever.
        int roomIndex = _rnd.RandomIndex(section.Rooms);
        room = section.Rooms[roomIndex];

        // Pick a position for the lever.
        int cornerIndex = _rnd.NextInt(4);
        var leverPosition = room.CornerPositions[cornerIndex];

        // Create the lever.
        var lever = new Lever(leverPosition, gate);
        room.AddEntity(lever);

        return true;
    }
}