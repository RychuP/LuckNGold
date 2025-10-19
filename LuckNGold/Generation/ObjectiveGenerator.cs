using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generates objectives for the map by hiding section keys. Objective in this case
/// is finding the keys, solving puzzles to free them up, if need be, and unlocking doors to 
/// all sections in order to be able to reach the entrance to the next level,
/// which is placed in the final room of the map.
/// </summary>
internal class ObjectiveGenerator() : GenerationStep("Objectives",
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"),
    new ComponentTypeTagPair(typeof(ItemList<Section>), "Sections"))
{
    readonly IEnhancedRandom _rnd = GlobalRandom.DefaultRNG;

    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths").Items;
        var sections = context.GetFirst<ItemList<Section>>("Sections").Items;

        // Create and hide the keys to section doors.
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            if (i == 0)
            {
                if (section.SingleRooms.Length >= 5)
                    PlaceKeyInRandomRoom(section);
                else
                    PlaceKeyInChests(section);
            }
            else if (i == 1)
            {

            }
        }

        yield break;
    }

    /// <summary>
    /// Places the key in one of the further rooms in the section.
    /// </summary>
    void PlaceKeyInRandomRoom(Section section)
    {
        var singleRoomCount = section.SingleRooms.Length;
        int startIndex = singleRoomCount / 2;

        int index;
        Room room;
        do
        {
            index = _rnd.NextInt(startIndex, singleRoomCount);
            room = section.SingleRooms[index];
        }
        while (room.Section!.Entrance == room);

        var keyPosition = room.Area.Center;
        var key = new Key(keyPosition, section.Gemstone);
        room.AddEntity(key);
    }

    /// <summary>
    /// Places a few chests in the section and the key in one of them.
    /// </summary>
    /// <param name="section"></param>
    void PlaceKeyInChests(Section section)
    {
        int chestsNeeded = 3;
        if (section.SingleRooms.Length < chestsNeeded)
            chestsNeeded = section.SingleRooms.Length;
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

            if (chestWithKeyIndex == i)
            {
                var key = new Key(Point.None, section.Gemstone);
                chest.Items.Add(key);
            }

            room.AddEntity(chest);
        }
    }

    void PlaceKeyInBarredRoom(Section section, IList<Section> sections)
    {

    }
}