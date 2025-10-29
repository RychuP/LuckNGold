namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Displays an appropriate screen for the current state of the program.
/// </summary>
internal class RootScreen : ScreenObject
{
    GameScreen? _gameScreen;
    readonly MainMenuScreen _mainMenuScreen;
    readonly GenerationScreen _generationScreen = new();
    readonly SettingsScreen _settingsScreen;

    public RootScreen()
    {
        _mainMenuScreen = new(this);
        _settingsScreen = new(this);
        Show<MainMenuScreen>();
    }

    public async void CreateNewGame()
    {
        Children.Clear();
        _generationScreen.Reset();
        Children.Add(_generationScreen);
        await Task.Run(() => _gameScreen = new GameScreen());
        Children.Clear();
        Children.Add(_gameScreen!);
    }

    public static void Exit()
    {
        Environment.Exit(0);
    }

    public static void None() { }

    public void Show<T>() where T : MenuScreen
    {
        Children.Clear();
        
        if (_mainMenuScreen is T) Children.Add(_mainMenuScreen);
        else if (_settingsScreen is T) Children.Add(_settingsScreen);

        if (Children.Count > 0 && Children[0] is MenuScreen menuScreen)
        {
            menuScreen.IsFocused = true;
            menuScreen.FocusFirstControl();
        }
    }
}