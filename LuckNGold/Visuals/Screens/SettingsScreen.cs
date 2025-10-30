using LuckNGold.Visuals.Components;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

internal class SettingsScreen : MenuScreen
{
    public const string ArrowButtonsText = "Arrow  Buttons";
    public const string NumpadButtonsText = "Numpad Buttons";
    public const string WasdButtonsText = "AWSD   Buttons";
    public const string ViButtonsText = "HJKL   Buttons";

    public SettingsScreen(RootScreen rootScreen) : base(rootScreen)
    {
        PrintTitle("-= Settings =-");
        DisplaySettingButtons();
    }

    void DisplaySettingButtons()
    {
        Controls.Clear();
        AddButton("Motion Controls", DisplayMotionControlSelectors,
            "Select between Arrow, Numpad, Wasd and Hjkl control schemes.");
        AddButton("Back", RootScreen.Show<MainMenuScreen>);
        FocusFirstControl();
    }

    void DisplayMotionControlSelectors()
    {
        Controls.Clear();
        AddCheckBox(ArrowButtonsText, true, CheckBox_Motions_OnIsSelectedChanged);
        AddCheckBox(NumpadButtonsText, true, CheckBox_Motions_OnIsSelectedChanged);
        AddCheckBox(WasdButtonsText, true, CheckBox_Motions_OnIsSelectedChanged);
        AddCheckBox(ViButtonsText, true, CheckBox_Motions_OnIsSelectedChanged);
        AddButton("Back", DisplaySettingButtons);
        FocusFirstControl();
    }

    void CheckBox_Motions_OnIsSelectedChanged(object? o, EventArgs e)
    {
        if (o is not CheckBox checkBox) return;

        // Make sure at least one set of motions is selected.
        if (!checkBox.IsSelected)
        {
            var checkboxes = Controls
                .Where(c => c is CheckBox)
                .Cast<CheckBox>();

            var unselectedCheckboxesCount = checkboxes
                .Where(c => c.IsSelected == false)
                .Count();

            if (checkboxes.Count() == unselectedCheckboxesCount)
            {
                checkBox.IsSelected = true;
                return;
            }
        }

        // Update motions.
        switch (checkBox.Text)
        {
            case ArrowButtonsText:
                Keybindings.ArrowMotionsEnabled = checkBox.IsSelected;
                RootScreen.UpdateKeybindings(checkBox);
                break;

            case NumpadButtonsText:
                Keybindings.NumpadMotionsEnabled = checkBox.IsSelected;
                RootScreen.UpdateKeybindings(checkBox);
                break;

            case WasdButtonsText:
                Keybindings.WasdMotionsEnabled = checkBox.IsSelected;
                RootScreen.UpdateKeybindings(checkBox);
                break;

            case ViButtonsText:
                Keybindings.ViMotionsEnabled = checkBox.IsSelected;
                RootScreen.UpdateKeybindings(checkBox);
                break;
        }
    }
}