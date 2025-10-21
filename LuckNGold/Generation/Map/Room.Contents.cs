namespace LuckNGold.Generation.Map;

partial class Room
{
    readonly List<Entity> _contents = [];
    /// <summary>
    /// List of entities placed in the room.
    /// </summary>
    public IReadOnlyList<Entity> Contents { get => _contents; }

    public void AddEntity(Entity entity)
    {
        if (entity.Position == Point.None)
            throw new ArgumentException("Entity needs to have a valid position.");

        if (!Bounds.Contains(entity.Position))
            throw new ArgumentException("Entity position is outside the bounds of the room.");

        if (!PositionIsFree(entity.Position))
            throw new ArgumentException("Another entity already at location.");

        _contents.Add(entity);
    }

    public bool RemoveEntity(Entity entity) =>
        _contents.Remove(entity);

    /// <summary>
    /// Checks if the are any entities with the given position in the <see cref="Contents"/>.
    /// </summary>
    /// <param name="position">Position to compare 
    /// with entities in the <see cref="Contents"/>.</param>
    /// <returns>True if the position is free and can accept <see cref="Entity"/>
    /// or false otherwise.</returns>
    public bool PositionIsFree(Point position) =>
        !Contents.Where(e => e.Position == position).Any();

    public bool Contains<T>() where T : Entity =>
        Contents.Where(e => e is T).Any();

    public Entity[] GetEntitiesAtY(int y)
    {
        if (y < Bounds.MinExtentY || y > Bounds.MaxExtentY)
            throw new ArgumentOutOfRangeException(nameof(y));
        return [.. Contents.Where(e => e.Position.Y == y)];
    }

    public Entity? GetEntityAt(Point position) =>
        Contents.Where(e => e.Position == position).FirstOrDefault();
}