using LuckNGold.Config;
using LuckNGold.Visuals.Windows.Panels;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Window for player character management.
/// </summary>
internal class CharacterWindow : Window
{
    readonly CharacterWindowTabControl _tabControl;

    public CharacterWindow() : base(Program.Width / 2, Program.Height / 2)
    {
        CalculatePosition();
        _tabControl = CreateTabs();
        Controls.Add(_tabControl);
        Hide();
    }

    void CalculatePosition()
    {
        int x = (Program.Width - Width) / 2;
        int y = (Program.Height - Height) / 2;
        Position = new Point(x, y);
    }

    CharacterWindowTabControl CreateTabs()
    {
        var equipmentTab = CreateEquipmentTab();
        var skillsTab = CreateSkillsTab();
        TabItem[] tabItems = [equipmentTab, skillsTab];
        return new CharacterWindowTabControl(tabItems, Width, Height);
    }

    TabItem CreateEquipmentTab()
    {
        var tabPanel = new EquipmentPanel(Width, Height);
        var tabItem = new TabItem("Equipment", tabPanel) { AutomaticPadding = 0 };
        return tabItem;
    }

    TabItem CreateSkillsTab()
    {
        var tabPanel = new SkillsPanel(Width, Height);
        var tabItem = new TabItem("Skills", tabPanel) { AutomaticPadding = 0 };
        return tabItem;
    }

    void SelectNextTab()
    {
        int index = _tabControl.ActiveTabIndex + 1;
        if (index >= _tabControl.Tabs.Count())
            index = 0;
        _tabControl.SetActiveTab(index);
    }

    protected override void OnVisibleChanged()
    {
        base.OnVisibleChanged();
        IsFocused = IsVisible;
        if (IsVisible)
            _tabControl.SetActiveTab(0);
    }

    public override bool ProcessKeyboard(Keyboard state)
    {
        if (state.IsKeyDown(Keys.LeftControl))
        {
            if (state.IsKeyPressed(Keybindings.CharacterWindow))
            {
                SelectNextTab();
            }
        }
        else
        {
            if (state.IsKeyPressed(Keys.Escape) || state.IsKeyPressed(Keybindings.CharacterWindow))
            {
                IsVisible = false;
            }
        }

        return base.ProcessKeyboard(state);
    }
}