using SadRogue.Integration;

namespace LuckNGold.World.Turns.Actions;

/// <summary>
/// Empty action that marks the beginning of turn for an entity.
/// </summary>
internal class Marker(RogueLikeEntity entity) : Action(entity, 0)
{ }