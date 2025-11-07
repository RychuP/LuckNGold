using SadConsole.UI.Controls;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Displays an appropriate screen for the current state of the program.
/// </summary>
internal class RootScreen : ScreenObject
{
    GameScreen? _gameScreen = null;

    /// <summary>
    /// Screen that is currently being displayed.
    /// </summary>
    public IScreenObject? CurrentScreen { get; private set; }

    /// <summary>
    /// Screen to be shown when Settings Screen is closed.
    /// </summary>
    public IScreenObject? ReturnScreen { get; private set; }

    /// <summary>
    /// Initializes the root screen.
    /// </summary>
    public void Init(object? o, GameHost host)
    {
        Game.Instance.Screen = this;

        // Create screens.
        Children.Add(new MainMenuScreen());
        Children.Add(new SettingsScreen());
        Children.Add(new MotionsSelectorScreen());
        Children.Add(new GenerationScreen());
        Children.Add(new PauseScreen());

        // Show main menu.
        HideAll();
        Show<MainMenuScreen>();
    }

    /// <summary>
    /// Creates a new gamescreen and adds it to the children. 
    /// Removes previous gamescreen if one existed.
    /// </summary>
    public async void CreateNewGame()
    {
        // Show generation screen.
        var generationScreen = Get<GenerationScreen>();
        generationScreen.Reset();
        Show(generationScreen);

        // Remove old gamescreen.
        if (_gameScreen != null && Children.Contains(_gameScreen))
            Children.Remove(_gameScreen);

        // Run generation.
        await Task.Run(() => _gameScreen = new GameScreen());

        // Show new game screen.
        Show(_gameScreen!);
    }

    /// <summary>
    /// Leaves the game.
    /// </summary>
    public static void Exit()
    {
        Environment.Exit(0);
    }

    /// <summary>
    /// Shows a screen (must be already added to children).
    /// </summary>
    /// <param name="screen">Screen to be shown.</param>
    public void Show(IScreenObject screen)
    {
        if (CurrentScreen == screen) return;

        if (CurrentScreen != null)
        {
            if (CurrentScreen is MenuScreen)
                CurrentScreen.SadComponents.Remove(MenuScreen.KeybindingsComponent);

            if (CurrentScreen is MainMenuScreen || CurrentScreen is PauseScreen)
                ReturnScreen = CurrentScreen;

            Hide(CurrentScreen);
        }

        CurrentScreen = screen;
        Children.Add(CurrentScreen);
        CurrentScreen.IsVisible = true;
        CurrentScreen.IsEnabled = true;
        CurrentScreen.IsFocused = true;

        if (CurrentScreen is MenuScreen menuScreen)
        {
            menuScreen.FocusFirstControl();
            menuScreen.SadComponents.Add(MenuScreen.KeybindingsComponent);
        }
    }

    /// <summary>
    /// Hides an individual screen.
    /// </summary>
    /// <param name="screen">Screen to be hidden.</param>
    static void Hide(IScreenObject screen)
    {
        screen.IsVisible = false;
        screen.IsEnabled = false;
    }

    /// <summary>
    /// Hides all screens.
    /// </summary>
    void HideAll()
    {
        foreach (var screen in Children)
            Hide(screen);
    }

    /// <summary>
    /// Shows screen of selected type.
    /// </summary>
    public void Show<T>() where T : class, IScreenObject
    {
        if (CurrentScreen is T) return;
        Show(Get<T>());
    }

    /// <summary>
    /// Gets screen of selected type.
    /// </summary>
    T Get<T>() where T : class, IScreenObject
    {
        return Children.Where(c => c is T).First() as T ??
            throw new InvalidOperationException("Screen could not be found.");
    }

    /// <summary>
    /// Tries to get screen of selected type.
    /// </summary>
    /// <typeparam name="T">Type of the screen to be shown.</typeparam>
    /// <param name="screen">Screen instance if found.</param>
    /// <returns>True if screen was found, false otherwise.</returns>
    bool TryGet<T>([NotNullWhen(true)] out T? screen) where T : class, IScreenObject
    {
        screen = Children.Where(c => c is T).FirstOrDefault() as T;
        return screen is not null;
    }

    /// <summary>
    /// Shows the screen that opened settings screen.
    /// </summary>
    public void ShowReturnScreen()
    {
        if (ReturnScreen is null)
            Show<MainMenuScreen>();
        else
            Show(ReturnScreen);
    }

    public void UpdateKeybindings(ControlBase control)
    {
        // Update menu screen keybindings.
        MenuScreen.KeybindingsComponent.UpdateKeybindings(control);

        // Update game screen keybindings.
        if (TryGet(out GameScreen? gameScreen))
        {
            gameScreen.UpdateKeybindings(control);
        }
    }
}