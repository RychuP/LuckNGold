using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Furnitures;
using LuckNGold.Generation.Items;
using LuckNGold.Generation.Items.Weapons.Swords;
using LuckNGold.Generation.Map;
using LuckNGold.World.Items.Enums;

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

        // Sample swords
        var swordPosition = firstRoom.Area.Center + Direction.UpLeft;
        var armingSword = new ArmingSword(swordPosition, Material.MoonSteel);
        firstRoom.AddEntity(armingSword);

        swordPosition += Direction.Right;
        var gladiusSword = new GladiusSword(swordPosition, Material.MoonSteel);
        firstRoom.AddEntity(gladiusSword);

        swordPosition += Direction.Right;
        var scimitarSword = new ScimitarSword(swordPosition, Material.MoonSteel);
        firstRoom.AddEntity(scimitarSword);

        yield break;
    }
}