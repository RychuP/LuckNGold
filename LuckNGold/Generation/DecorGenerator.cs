using GoRogue.Components;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using LuckNGold.Generation.Map;

namespace LuckNGold.Generation;

/// <summary>
/// Generator that creates decorators to fill empty space in dungeon rooms.
/// </summary>
internal class DecorGenerator() : GenerationStep("Decorators",
    new ComponentTypeTagPair(typeof(ItemList<Room>), "Rooms"))
{
    protected override IEnumerator<object?> OnPerform(GenerationContext context)
    {
        throw new NotImplementedException();
    }
}