using LuckNGold.Config;
using LuckNGold.Resources;
using LuckNGold.Visuals.Components;
using LuckNGold.Visuals.Consoles;
using LuckNGold.Visuals.Controls;
using LuckNGold.Visuals.Screens;
using LuckNGold.World.Monsters.Interfaces;
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
    readonly GameScreen _gameScreen;

    // to be changed back into a field...
    public readonly EquipmentPage EquipmentPage;

    public CharacterWindow(GameScreen gameScreen) : 
        base(GameSettings.CharacterWindowWidth, GameSettings.CharacterWindowHeight)
    {
        _gameScreen = gameScreen;
        CalculatePosition();

        // Create tab control.
        _tabControl = CreateTabs();
        _tabControl.ActiveTabItemChanged += TabControl_OnActiveTabItemChanged;
        Controls.Add(_tabControl);

        // Create equipment page for the equipment tab.
        var equipmentComponent = gameScreen.Player.AllComponents.GetFirst<IEquipment>();
        EquipmentPage = new(equipmentComponent);
        Children.Add(EquipmentPage);

        // Create keybindings component.
        var keybindingsComponent = new CharacterWindowKeybindings(gameScreen, this);
        SadComponents.Add(keybindingsComponent);

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

    public void SelectNextTab()
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
            EquipmentPage.IsVisible = true;
        }
        else if (e.OldValue != null && e.OldValue.Header == Strings.EquipmentTabName)
        {
            EquipmentPage.IsVisible = false;
        }
    }

    protected override void OnVisibleChanged()
    {
        base.OnVisibleChanged();
        IsFocused = IsVisible;
        if (IsVisible)
        {
            _tabControl.SetActiveTab(0);
            EquipmentPage.CharacterLoadout.SelectSlot(0);
        }
    }
}