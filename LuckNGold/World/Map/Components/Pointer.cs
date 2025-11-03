using LuckNGold.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Map.Components;

internal class Pointer : AnimatedRogueLikeEntity
{
    public Pointer() : base("Pointer", true, GameMap.Layer.Monsters)
    {

    }

    protected override void OnVisibleChanged()
    {
        IsVisible = true;
    }
}