using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Map;
using LuckNGold.Tests;
using LuckNGold.World.Items.Enums;
using ShaiRandom.Generators;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that places sample entities in the entrance room of the current level.
/// </summary>
internal class SamplesGenerator() : GenerationStep("Samples",
    new ComponentTypeTagPair(typeof(ItemList<Section>), "Sections"))
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        var sections = context.GetFirst<ItemList<Section>>("Sections").Items;
        var firstRoom = sections[0].Entrance;
        var exit = firstRoom.Exits.First();

        // Place gate.
        var gate = new Gate(exit.Position, exit.Direction);
        firstRoom.AddEntity(gate);

        // Place lever.
        int cornerIndex = GlobalRandom.DefaultRNG.NextInt(4);
        var leverPosition = firstRoom.CornerPositions[cornerIndex];
        var lever = new Lever(leverPosition, gate);
        firstRoom.AddEntity(lever);

        // Calculate chest position.
        Point chestPosition;
        do
        {
            cornerIndex = GlobalRandom.DefaultRNG.NextInt(4);
            chestPosition = firstRoom.CornerPositions[cornerIndex];
        }
        while (chestPosition == leverPosition);
        chestPosition += cornerIndex == 0 || cornerIndex == 1 ?
            Direction.Down : Direction.Up;

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