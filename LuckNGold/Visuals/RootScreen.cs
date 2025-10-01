using LuckNGold.Visuals.Screens;

namespace LuckNGold.Visuals;

/// <summary>
/// RootScreen displays an appropriate screen for the current state of the program.
/// </summary>
internal class RootScreen : ScreenObject
{
    GameScreen? _gameScreen;
    readonly MainMenuScreen _mainMenuScreen;
    readonly GenerationScreen _generationScreen = new();

    public RootScreen()
    {
        _mainMenuScreen = new(this);
        DisplayMainMenu();
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

    public void DisplayMainMenu()
    {
        Children.Clear();
        Children.Add(_mainMenuScreen);
    }
}