using LuckNGold.Config;
using LuckNGold.Resources;
using LuckNGold.Visuals.Consoles;
using LuckNGold.Visuals.Controls;
using LuckNGold.World.Monsters.Interfaces;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Window for player character management.
/// </summary>
internal class CharacterWindow : Window
{
    readonly CharacterWindowTabControl _tabControl;
    readonly EquipmentPage _equipmentPage;

    public CharacterWindow(RogueLikeEntity player) : base(GameSettings.CharacterWindowWidth, 
        GameSettings.CharacterWindowHeight)
    {
        CalculatePosition();
        _tabControl = CreateTabs();
        var equipmentComponent = player.AllComponents.GetFirst<IEquipment>();
        _equipmentPage = new(equipmentComponent);
        Controls.Add(_tabControl);
        Children.Add(_equipmentPage);
        _tabControl.ActiveTabItemChanged += TabControl_OnActiveTabItemChanged;
        Hide();
    }

    void CalculatePosition()
    {
        int x = (GameSettings.Width - Width) / 2;
        int y = (GameSettings.Height - Height) / 2;
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
        var tabItem = new TabItem(Strings.EquipmentTabName, tabPanel) { AutomaticPadding = 0 };
        return tabItem;
    }

    TabItem CreateSkillsTab()
    {
        var tabPanel = new SkillsPanel(Width, Height);
        var tabItem = new TabItem(Strings.SkillsTabName, tabPanel) { AutomaticPadding = 0 };
        return tabItem;
    }

    void SelectNextTab()
    {
        int index = _tabControl.ActiveTabIndex + 1;
        if (index >= _tabControl.Tabs.Count())
            index = 0;
        _tabControl.SetActiveTab(index);
    }

    void TabControl_OnActiveTabItemChanged(object? o, ValueChangedEventArgs<TabItem?> e)
    {
        if (e.NewValue != null && e.NewValue.Header == Strings.EquipmentTabName)
        {
            _equipmentPage.IsVisible = true;
        }
        else if (e.OldValue != null && e.OldValue.Header == Strings.EquipmentTabName)
        {
            _equipmentPage.IsVisible = false;
        }
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