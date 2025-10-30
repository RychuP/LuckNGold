using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that places entities in the first room of the current level.
/// </summary>
internal class FirstRoomGenerator() : GenerationStep("FirstRoom",
    new ComponentTypeTagPair(typeof(ItemList<Section>), "Sections"))
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var sections = context.GetFirst<ItemList<Section>>("Sections").Items;
        var firstRoom = sections[0].Entrance;
        var exit = firstRoom.Exits.First();

        // Place gate.
        var gate = new Gate(exit);
        firstRoom.AddEntity(gate);

        // Place lever.
        var leverPosition = firstRoom.GetRandomCorner();
        var lever = new Lever(leverPosition, gate);
        firstRoom.AddEntity(lever);

        // Calculate chest position.
        Point cornerPosition;
        do cornerPosition = firstRoom.GetRandomCorner();
        while (cornerPosition == leverPosition);
        var cornerNeighbours = firstRoom.GetCornerNeighbours(cornerPosition);
        var chestPosition = cornerNeighbours[1];

        // Place chest.
        var chest = new Chest(chestPosition);
        firstRoom.AddEntity(chest);

        // Add coins to chest.
        for (int j = 0; j < 5; j++)
        {
            var coin = new Coin(Point.None);
            chest.Items.Add(coin);
        }

        yield break;
    }
}