using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;

namespace LuckNGold.World.Turns;

/// <summary>
/// Manages passage of time and turn taking by monster entities.
/// </summary>
internal class TurnManager
{
    public event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? CurrentEntityChanged;
    public event EventHandler? TickEncountered;

    readonly GameMap _map;
    readonly Queue<IEvent> _events = [];
    readonly RogueLikeEntity _player;

    RogueLikeEntity? _currentEntity;
    public RogueLikeEntity? CurrentEntity
    {
        get => _currentEntity;
        protected set
        {
            if (value == _currentEntity) return;
            var prevEntity = _currentEntity;
            _currentEntity = value;
            OnCurrentEntityChanged(prevEntity, _currentEntity);
        }
    }

    public TurnManager(GameMap map)
    {
        _map = map;

        _player = map.Monsters
            .Where(e => e.Name == "Player")
            .First();
        _events.Enqueue(new Marker(_player));
        _currentEntity = _player;

        foreach (var monster in map.Monsters)
        {
            if (monster.Name != "Player")
            {
                _events.Enqueue(new Marker(monster));
            }
        }

        _events.Enqueue(new Tick(map));
    }

    public void Add(IAction action)
    {
        var timeTracker = action.Entity.AllComponents.GetFirst<ITimeTracker>();
        Evaluate(action, timeTracker);
    }

    void Evaluate(IAction action, ITimeTracker timeTracker)
    {
        // Check if there is enough time to execute the action immediately.
        if (action.Time - timeTracker.Time <= 0)
        {
            // Execute the action.
            if (action.Execute())
            {
                // Reduce available time by the amount of time it took to execute the action.
                timeTracker.Time -= action.Time;

                if (timeTracker.Time == 0)
                {
                    // Add marker to the stack and complete entity's turn.
                    _events.Enqueue(new Marker(action.Entity));

                    // Move on with the turn.
                    PassTime();
                }
            }
        }

        // Not enough time to execute selected action.
        else
        {
            action.Time -= timeTracker.Time;
            timeTracker.Time = 0;

            // Schedule the action to be taken later on.
            _events.Enqueue(action);

            // Move on with the turn.
            PassTime();
        }
    }

    public void PassTime()
    {
        var @event = _events.Dequeue();
        
        // End of turn. Replenish time.
        if (@event is ITick tick)
        {
            tick.Reset();
            OnTickEncountered();
            _events.Enqueue(tick);
            CurrentEntity = null;
        }

        // Evaluate actions.
        else if (@event is IAction action)
        {
            var entity = action.Entity;
            var timeTracker = entity.AllComponents.GetFirst<ITimeTracker>();
            Evaluate(action, timeTracker);

            // If the entity has time left, mark it as current entity taking turn.
            if (timeTracker.Time > 0)
            {
                CurrentEntity = entity;
            }
        }
    }

    void OnTickEncountered()
    {
        TickEncountered?.Invoke(this, EventArgs.Empty);
    }

    void OnCurrentEntityChanged(RogueLikeEntity? prevEntity, RogueLikeEntity? newEntity)
    {
        var args = new ValueChangedEventArgs<RogueLikeEntity?>(prevEntity, newEntity);
        CurrentEntityChanged?.Invoke(this, args);
    }
}