using SadConsole.UI.Controls;
using System.Diagnostics.CodeAnalysis;

namespace LuckNGold.Visuals.Screens;

/// <summary>
/// Displays an appropriate screen for the current state of the program.
/// </summary>
internal class RootScreen : ScreenObject
{
    /// <summary>
    /// Screen that is currently being displayed.
    /// </summary>
    public IScreenObject? CurrentScreen { get; private set; }

    /// <summary>
    /// Screen to be shown when Settings Screen is closed.
    /// </summary>
    public IScreenObject? ReturnScreen { get; private set; }

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

    public async void CreateNewGame()
    {
        // Show generation screen.
        var generationScreen = Get<GenerationScreen>();
        generationScreen.Reset();
        Show(generationScreen);

        // Remove old gamescreen.
        if (TryGet(out GameScreen? oldGameScreen))
        {
            Children.Remove(oldGameScreen);
            if (ReturnScreen == oldGameScreen)
                ReturnScreen = Get<MainMenuScreen>();
        }

        // Run generation.
        GameScreen? newGameScreen = null;
        await Task.Run(() => newGameScreen = new GameScreen());

        // Show new game screen.
        Show(newGameScreen!);
    }

    public static void Exit()
    {
        Environment.Exit(0);
    }

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

    static void Hide(IScreenObject screen)
    {
        screen.IsVisible = false;
        screen.IsEnabled = false;
    }

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

    bool TryGet<T>([NotNullWhen(true)] out T? screen) where T : class, IScreenObject
    {
        screen = Children.Where(c => c is T).FirstOrDefault() as T;
        return screen is not null;
    }

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