using LuckNGold.Visuals.Screens;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadConsole.Components;
using SadRogue.Integration;

namespace LuckNGold.World.Turns;

/// <summary>
/// Manages passage of time and turn taking by monster entities.
/// </summary>
internal class TurnManager : UpdateComponent
{
    public event EventHandler<ValueChangedEventArgs<RogueLikeEntity?>>? CurrentEntityChanged;
    public event EventHandler<int>? TurnCounterChanged;
    public event EventHandler<IAction>? ActionAdded;

    const int TurnCounterMax = 100000000;

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

            // Check that value only increases in increments of 1.
            if (value < _turnCounter || value > _turnCounter + 1)
                throw new ArgumentException("Value should only increase in 1 increments.");

            // Roll over the counter if max is reached.
            if (value >= TurnCounterMax)
                value = 0;

            _turnCounter = value;
            OnTurnCounterChanged();
        }
    }

    RogueLikeEntity? _currentEntity;
    /// <summary>
    /// Entity currently taking turn.
    /// </summary>
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

    public override void Update(IScreenObject host, TimeSpan delta)
    {
        while (true)
        {
            if (CurrentEntity is null)
            {
                PassTime();
            }
            else
            {
                var onionComponent = CurrentEntity.AllComponents.GetFirst<IOnion>();
                if (onionComponent.IsBumping)
                    return;

                var timeTracker = CurrentEntity.AllComponents.GetFirst<ITimeTracker>();
                if (timeTracker.Time <= 0)
                {
                    PassTime();
                }
                else
                {
                    // Enemy's turn.
                    if (CurrentEntity.Name != "Player")
                    {
                        var enemyAI = CurrentEntity.AllComponents.GetFirst<IEnemyAI>();

                        // Keep getting actions until there is time left or bumping starts.
                        while (timeTracker.Time > 0)
                        {
                            var action = enemyAI.GetAction();
                            Add(action);

                            // Check if the action initiated bumping.
                            if (onionComponent.IsBumping)
                                return;
                        }

                        // No more time left. Move on to next event.
                        PassTime();
                    }
                    // Player's turn.
                    else
                    {
                        // Not much we can do here. Waiting for input.
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Triggers ActionAdded event and either executes or enqueues the action depending on time available.
    /// </summary>
    /// <param name="action">Action to be executed or enqueud.</param>
    public void Add(IAction action)
    {
        OnActionAdded(action);
        Evaluate(action);
    }

    void Evaluate(IAction action)
    {
        if (CurrentEntity != action.Source)
            throw new InvalidOperationException("Action source entity is not Current Entity.");

        var timeTracker = CurrentEntity.AllComponents.GetFirst<ITimeTracker>();

        // Check if there is enough time to execute the action immediately.
        if (action.Time - timeTracker.Time <= 0)
        {
            // Execute the action.
            if (action.Execute())
            {
                // Reduce available time by the amount of time it took to execute the action.
                timeTracker.Time -= action.Time;
            }
            // EnemyAI should not produce actions that fail but in case they do, pass their turn.
            else if (CurrentEntity.Name != "Player")
            {
                timeTracker.GetWaitAction().Execute();
            }

            if (timeTracker.Time == 0)
            {
                // Create a marker for the next entity's turn.
                var marker = new Marker(action.Source);

                // Add marker to the stack and finish entity's turn.
                _events.Enqueue(marker);
            }
        }

        // Not enough time to execute selected action.
        else
        {
            // Reduce action time cost by whatever time the entity has left.
            action.Time -= timeTracker.Time;
            timeTracker.Time = 0;

            // Schedule the action to be taken later on.
            _events.Enqueue(action);
        }
    }

    /// <summary>
    /// Passes time by moving to the next queued event.
    /// </summary>
    void PassTime()
    {
        // Get next event queued.
        var @event = _events.Dequeue();
        
        // End of turn. 
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
            CurrentEntity = action.Source;
            Evaluate(action);
        }
    }

    void OnCurrentEntityChanged(RogueLikeEntity? prevEntity, RogueLikeEntity? newEntity)
    {
        var args = new ValueChangedEventArgs<RogueLikeEntity?>(prevEntity, newEntity);
        CurrentEntityChanged?.Invoke(this, args);
    }
    
    void OnTurnCounterChanged()
    {
        TurnCounterChanged?.Invoke(this, TurnCounter);
    }

    void OnActionAdded(IAction action)
    {
        ActionAdded?.Invoke(this, action);
    }
}