using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Furniture;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.World.Items.Enums;
using SadRogue.Integration;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generates objectives for the map by hiding section keys. Objective in this case
/// is finding the keys, solving puzzles to free them up, if need be, and unlocking doors to 
/// all sections in order to be able to reach the entrance to the next level,
/// which is placed in the final room of the map.
/// </summary>
internal class ObjectiveGenerator() : GenerationStep("Objectives",
    new ComponentTypeTagPair(typeof(ItemList<Item>), "Items"),
    new ComponentTypeTagPair(typeof(ItemList<RoomPath>), "Paths"),
    new ComponentTypeTagPair(typeof(ItemList<Section>), "Sections"))
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var paths = context.GetFirst<ItemList<RoomPath>>("Paths");
        var sections = context.GetFirst<ItemList<Section>>("Sections");
        var furniture = context.GetFirst<ItemList<Entity>>("Furniture");
        var items = context.GetFirst<ItemList<Item>>("Items");

        // Get doors created so far.
        var doors = furniture.Items.Where(e => e is Door).Cast<Door>().ToArray();

        var rnd = GlobalRandom.DefaultRNG;
        Room room;
        Key key;
        Coin coin;

        // Create matching door keys.
        foreach (var door in doors)
        {
            if (door.Lock is not Lock @lock)
                continue;

            var section = sections.Items
                .Where(s => s.Gemstone == @lock.Gemstone)
                .First();

            switch (@lock.Gemstone)
            {
                // Place the key in one of the further single rooms.
                case Gemstone.Onyx:
                    var singleRoomCount = section.SingleRooms.Length;
                    int startIndex = singleRoomCount / 2;

                    int index;
                    do
                    {
                        index = rnd.NextInt(startIndex, singleRoomCount);
                        room = section.SingleRooms[index];
                    }
                    while (room.Section!.Entrance == room);

                    var keyPosition = room.Area.Center;
                    key = new Key(keyPosition, @lock.Gemstone);
                    items.Add(key, Name);
                    break;

                // Place a few chests and the key in one of them
                case Gemstone.Amber:
                    int chestsNeeded = 3;
                    if (section.SingleRooms.Length < chestsNeeded)
                        chestsNeeded = section.SingleRooms.Length;
                    int chestWithKeyIndex = rnd.NextInt(chestsNeeded);
                    var roomIndicesUsed = new List<int>(chestsNeeded);

                    // Create chests.
                    for (int i = 0; i < chestsNeeded; i++)
                    {
                        int roomIndex;
                        do roomIndex = rnd.RandomIndex(section.SingleRooms);
                        while (roomIndicesUsed.Contains(roomIndex));
                        roomIndicesUsed.Add(roomIndex);
                        room = section.SingleRooms[roomIndex];

                        // Create current chest.
                        var chestPosition = room.Area.Center;
                        var chest = new Chest(chestPosition);

                        // Put some coins in the chest.
                        int coinsNeeded = rnd.NextInt(5);
                        for (int j = 0; j < coinsNeeded; j++)
                        {
                            coin = new Coin(Point.None);
                            chest.Items.Add(coin);
                        }

                        if (chestWithKeyIndex == i)
                        {
                            key = new Key(Point.None, @lock.Gemstone);
                            chest.Items.Add(key);
                        }

                        furniture.Add(chest, Name);
                    }
                    break;
            }
        }

        yield break;
    }
}