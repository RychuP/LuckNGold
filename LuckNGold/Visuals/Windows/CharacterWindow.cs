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
    public CharacterWindow() : base(Program.Width / 2, Program.Height / 2)
    {
        CalculatePosition();
        AddTabs();
        Hide();
    }

    void CalculatePosition()
    {
        int x = (Program.Width - Width) / 2;
        int y = (Program.Height - Height) / 2;
        Position = new Point(x, y);
    }

    void AddTabs()
    {
        var equipmentTab = CreateEquipmentTab();
        var skillsTab = CreateSkillsTab();
        TabItem[] tabItems = [equipmentTab, skillsTab];
        TabControl tab = new CharacterWindowTabControl(tabItems, Width, Height);
        Controls.Add(tab);
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

    protected override void OnVisibleChanged()
    {
        base.OnVisibleChanged();
        IsFocused = IsVisible;
    }

    public override bool ProcessKeyboard(Keyboard state)
    {
        if (state.IsKeyPressed(Keys.Escape) || state.IsKeyPressed(Keybindings.CharacterWindow))
        {
            IsVisible = false;
        }

        return base.ProcessKeyboard(state);
    }
}