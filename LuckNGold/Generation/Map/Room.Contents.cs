namespace LuckNGold.Generation.Map;

// Properties and methods relating to room contents (entities added to the room).
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

    /// <summary>
    /// Gets all entities which are placed on the given Y coordinate.
    /// </summary>
    /// <param name="y">Y coordinate of the map.</param>
    /// <returns>Array of entities found.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Entity[] GetEntitiesAtY(int y)
    {
        if (y < Bounds.MinExtentY || y > Bounds.MaxExtentY)
            throw new ArgumentOutOfRangeException(nameof(y));
        return [.. Contents.Where(e => e.Position.Y == y)];
    }

    /// <summary>
    /// Gets an entity at the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Entity if found, otherwise null.</returns>
    public Entity? GetEntityAt(Point position) =>
        Contents.Where(e => e.Position == position).FirstOrDefault();

    /// <summary>
    /// Gets the first found entity of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the entity.</typeparam>
    /// <returns>Entity if found, otherise null.</returns>
    public T? GetEntity<T>() where T : Entity =>
        Contents.Where(e => e is T).FirstOrDefault() as T;
}