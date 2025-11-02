using LuckNGold.Visuals.Components;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Screens;

internal class SettingsScreen : MenuScreen
{
    public const string ArrowButtonsText = "Arrow  Buttons";
    public const string NumpadButtonsText = "Numpad Buttons";
    public const string FPSButtonsText = "FPS    Buttons";
    public const string ViButtonsText = "VI     Buttons";

    const string MotionControlsTitle = "Motion Controls";

    const string MotionControlsInstruction = "Select between Arrow, Numpad, " +
        "FPS and Vi control schemes";

    const string FourWay = "Four way controls:";
    const string EightWay = "Eight way controls:";
    const string MotionCheckBoxInstruction = "At least one set of motions " +
            "must be selected";
    const string ArrowCheckBoxInstruction = $"{FourWay} left, right, " +
        "up and down cardinal directions";
    const string ViCheckBoxInstruction = $"{EightWay} HJKL cardinal, BNYU diagonal directions";
    const string NumpadInstruction = $"{EightWay} 2468 cardinal, 1379 diagonal directions";
    const string FPSInstruction = $"{FourWay} WASD cardinal directions";

    public SettingsScreen() : base()
    {
        Name = "Settings";
        DisplaySettingButtons();
    }

    void DisplaySettingButtons()
    {
        EraseTitle();
        PrintTitle(Name);

        Controls.Clear();
        AddButton(MotionControlsTitle, DisplayMotionControlSelectors, MotionControlsInstruction);
        AddButton("Back", Program.RootScreen.ShowPrevScreen, 
            Program.RootScreen.GetBackButtonInstruction());

        FocusFirstControl();
    }

    void DisplayMotionControlSelectors()
    {
        PrintTitle(MotionControlsTitle);

        Controls.Clear();
        AddCheckBox(ArrowButtonsText, Keybindings.ArrowMotionsEnabled, 
            CheckBox_Motions_OnIsSelectedChanged,
            MotionCheckBoxInstruction, ArrowCheckBoxInstruction);

        AddCheckBox(NumpadButtonsText, Keybindings.NumpadMotionsEnabled, 
            CheckBox_Motions_OnIsSelectedChanged,
            MotionCheckBoxInstruction, NumpadInstruction);

        AddCheckBox(FPSButtonsText, Keybindings.FPSMotionsEnabled, 
            CheckBox_Motions_OnIsSelectedChanged,
            MotionCheckBoxInstruction, FPSInstruction);

        AddCheckBox(ViButtonsText, Keybindings.ViMotionsEnabled, 
            CheckBox_Motions_OnIsSelectedChanged,
            MotionCheckBoxInstruction, ViCheckBoxInstruction);

        AddButton("Back", DisplaySettingButtons, BackButtonInstruction);

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
                break;

            case NumpadButtonsText:
                Keybindings.NumpadMotionsEnabled = checkBox.IsSelected;
                break;

            case FPSButtonsText:
                Keybindings.FPSMotionsEnabled = checkBox.IsSelected;
                break;

            case ViButtonsText:
                Keybindings.ViMotionsEnabled = checkBox.IsSelected;
                break;
        }

        Program.RootScreen.UpdateKeybindings(checkBox);
    }
}