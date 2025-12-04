using LuckNGold.Visuals.Screens;
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
    public event EventHandler? TurnCounterChanged;
    public event EventHandler<IAction>? ActionAdded;

    /// <summary>
    /// Queue of events happenning in the game (pretty much a turn queue for all monster entities)
    /// </summary>
    readonly Queue<IEvent> _events = [];

    int _turnCounter = 0;
    public int TurnCounter
    {
        get => _turnCounter;
        private set
        {
            if (_turnCounter == value) return;
            if (value < _turnCounter || value > _turnCounter + 1)
                throw new ArgumentException("Value should only increase in 1 increments.");
            _turnCounter = value;
            OnTurnCounterChanged();
        }
    }

    RogueLikeEntity? _currentEntity;
    public RogueLikeEntity? CurrentEntity
    {
        get => _currentEntity;
        private set
        {
            if (value == _currentEntity) return;
            var prevEntity = _currentEntity;
            _currentEntity = value;
            OnCurrentEntityChanged(prevEntity, _currentEntity);
        }
    }

    public TurnManager(GameScreen gameScreen)
    {
        // Mark player as the first to move.
        _events.Enqueue(new Marker(gameScreen.Player));
        _currentEntity = gameScreen.Player;

        // Add empty actions for all monster entities 
        foreach (var monster in gameScreen.Map.Monsters)
        {
            if (monster.Name != "Player")
                _events.Enqueue(new Marker(monster));
        }

        // Add tick event that marks the end of turn.
        _events.Enqueue(new Tick(gameScreen.Map));
    }

    /// <summary>
    /// Tries to complete the action if there is enough time points
    /// or adds it to the queue of events to complete it later in the future.
    /// </summary>
    /// <param name="action">Action to be evaluated.</param>
    public void Add(IAction action)
    {
        var timeTracker = action.Source.AllComponents.GetFirst<ITimeTracker>();
        Evaluate(action, timeTracker);
        OnActionAdded(action);
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
                    _events.Enqueue(new Marker(action.Source));

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

    /// <summary>
    /// Passes time by moving to the next queued event.
    /// </summary>
    public void PassTime()
    {
        // Check onion component of the current entity is not playing a bump animation.
        if (CurrentEntity is not null &&
            CurrentEntity.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent &&
            onionComponent.IsBumping)
        {
            // Delay passing time until bump animation is complete.
            onionComponent.IsBumpingChanged += IOnion_OnIsBumpingChanged;
            return;
        }

        // Get next event queued.
        var @event = _events.Dequeue();
        
        // End of turn. Replenish time.
        if (@event is ITick tick)
        {
            tick.Reset();
            TurnCounter++;
            _events.Enqueue(tick);
            CurrentEntity = null;
        }

        // Evaluate actions.
        else if (@event is IAction action)
        {
            var entity = action.Source;
            var timeTracker = entity.AllComponents.GetFirst<ITimeTracker>();
            Evaluate(action, timeTracker);

            // If the entity has time left, mark it as current entity taking turn.
            if (timeTracker.Time > 0)
            {
                CurrentEntity = entity;
            }
        }
    }

    void OnCurrentEntityChanged(RogueLikeEntity? prevEntity, RogueLikeEntity? newEntity)
    {
        var args = new ValueChangedEventArgs<RogueLikeEntity?>(prevEntity, newEntity);
        CurrentEntityChanged?.Invoke(this, args);
    }

    void OnTurnCounterChanged()
    {
        TurnCounterChanged?.Invoke(this, EventArgs.Empty);
    }

    void OnActionAdded(IAction action)
    {
        ActionAdded?.Invoke(this, action);
    }

    void IOnion_OnIsBumpingChanged(object? o, EventArgs e)
    {
        if (o is not IOnion onionComponent)
            throw new InvalidOperationException("Handler attached to a wrong component.");

        if (!onionComponent.IsBumping)
        {
            onionComponent.IsBumpingChanged -= IOnion_OnIsBumpingChanged;
            PassTime();
        }
    }
}