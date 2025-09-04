using GoRogue.GameFramework;
using LuckNGold.World.Entities.Actors;
using LuckNGold.World.Entities.Items;
using SadRogue.Primitives.SpatialMaps;

namespace LuckNGold;

class ItemEventArgs(ICarryable item) : EventArgs
{
    public ICarryable Item { get; init; } = item;
}

class FailedActionEventArgs(string message) : EventArgs
{
    public string Message { get; init; } = message;
}

class CombatEventArgs(int damage, Actor target) : EventArgs
{
    public int Damage { get; init; } = damage;
    public Actor Target { get; init; } = target;
}

class ConsumedEventArgs(IConsumable item) : EventArgs
{
    public IConsumable Item { get; init; } = item;
}

class MapGeneratedEventArgs(IReadOnlySpatialMap<IGameObject> actors) : EventArgs
{
    public IReadOnlySpatialMap<IGameObject> Actors { get; init; } = actors;
}