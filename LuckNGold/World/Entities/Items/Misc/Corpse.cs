using LuckNGold.World.Entities.Actors;

namespace LuckNGold.World.Entities.Items.Misc;

internal class Corpse : Item
{
    public Corpse(Actor actor) : base($"Corpse of the {actor.Name}", '%', actor.Color)
    {
        Position = actor.Position;
    }
}