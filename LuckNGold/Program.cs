using LuckNGold.Screens;
using SadConsole.Configuration;

Settings.WindowTitle = "Luck N' Gold";

Builder
    .GetBuilder()
    .SetWindowSizeInCells(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true)
    .SetDefaultFontSize(IFont.Sizes.Two)
    .Run();