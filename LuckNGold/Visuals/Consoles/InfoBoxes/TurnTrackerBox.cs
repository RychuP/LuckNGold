using LuckNGold.Config;
using LuckNGold.Resources;
using LuckNGold.World.Turns;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Consoles.InfoBoxes;

/// <summary>
/// Displays turn counter and a visual representation of length and frequency of player and enemy turns.
/// </summary>
internal class TurnTrackerBox : InfoBox
{
    readonly int _trackerMaxLength;
    readonly EnemyTurnClock _clock;
    ColoredString _tracker = new();
    int _playerColorIndex = 0;
    int _enemyColorIndex = 0;

    public TurnTrackerBox(TurnManager turnManager) : 
        base("Turn 00K Tracker", Strings.TurnTrackerBoxDescription)
    {
        turnManager.CurrentEntityChanged += TurnManager_OnCurrentEntityChanged;
        turnManager.TurnCounterChanged += TurnManager_OnTurnCounterChanged;

        _clock = new EnemyTurnClock() { Position = (Width - 1, 1) };
        Children.Add(_clock);

        _trackerMaxLength = Width - 6;

        ShowPlayerTurn();
        Surface.Print(0, 1, "000");
    }

    // Prints the string holding glyphs representing player and enemy turns.
    void PrintTurnTracker()
    {
        if (_tracker.Length > _trackerMaxLength)
        {
            //Surface.Print(0, 1, string.Empty.PadRight(_trackerMaxLength));
            int startIndex = _tracker.Length - _trackerMaxLength;
            _tracker = _tracker.SubString(startIndex, _trackerMaxLength);
        }

        Surface.Print(4, 1, _tracker);
    }

    void ShowPlayerTurn()
    {
        var color = Theme.PlayerTurnColors[_playerColorIndex++];
        if (_playerColorIndex >= Theme.PlayerTurnColors.Length)
            _playerColorIndex = 0;
        AddGlyph("P", color);
    }

    void ShowEnemyTurn()
    {
        var color = Theme.EnemyTurnColors[_enemyColorIndex++];
        if (_enemyColorIndex >= Theme.EnemyTurnColors.Length)
            _enemyColorIndex = 0;
        AddGlyph("E", color);
    }

    void ShowTurnEnd() =>
        AddGlyph(":", Color.RosyBrown);

    // Adds glyph to the end of the tracker.
    void AddGlyph(string letter, Color color)
    {
        _tracker += letter.CreateColored(color);
        PrintTurnTracker();
    }

    // Displays turn counter.
    void TurnManager_OnTurnCounterChanged(object? o, int counter)
    {
        ShowTurnEnd();

        // Print hundreds of the turn counter.
        int hundreds = counter % 1000;
        Surface.Print(0, 1, $"{hundreds}".PadLeft(3, '0'));

        // Print thousands and higher digits of the turn counter.
        string roundedCounter = "00K";
        if (counter >= 1000 && counter < 100000)
        {
            SetRoundedCounter(1000, 'K');
        }
        else if (counter >= 100000 && counter < 10000000)
        {
            SetRoundedCounter(100000, 'H');
        }
        else if (counter >= 10000000 && counter < 100000000)
        {
            SetRoundedCounter(1000000, 'M');
        }
        Surface.Print(5, 0, roundedCounter);

        void SetRoundedCounter(int divider, char symbol)
        {
            double roundedDouble = Math.Floor((double)counter / divider);
            int roundedNumber = Convert.ToInt32(roundedDouble);
            roundedCounter = $"{roundedNumber}".PadLeft(2, '0') + symbol;
        }
    }

    void TurnManager_OnCurrentEntityChanged(object? o, ValueChangedEventArgs<RogueLikeEntity?> e)
    {
        if (e.NewValue is null) return;

        if (e.NewValue.Name == "Player")
        {
            _clock.Stop();
            ShowPlayerTurn();
        }
        else
        {
            if (!_clock.IsRunning)
                _clock.Start();

            // Skip adding more Es on subsequent enemy turns.
            if (_tracker[^1].Glyph != 'E')
                ShowEnemyTurn();
        }
    }
}