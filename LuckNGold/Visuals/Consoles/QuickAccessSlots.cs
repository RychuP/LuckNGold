using LuckNGold.Config;
using LuckNGold.World.Monsters.Components;

namespace LuckNGold.Visuals.Consoles;

/// <summary>
/// Slots that display contents of <see cref="QuickAccessComponent"/> attached to the player.
/// </summary>
internal class QuickAccessSlots : SlotHolder
{
    /// <summary>
    /// Initializes an instance of <see cref="QuickAccessSlots"/> class.
    /// </summary>
    /// <param name="quickAccess">Source component.</param>
    public QuickAccessSlots(QuickAccessComponent quickAccess) : base(5, 10, 1, 1)
    {
        quickAccess.Changed += QuickAccess_OnSlotChanged;
        CreateSlots();
    }

    /// <summary>
    /// Creates slots up the maximum specified in the component.
    /// </summary>
    void CreateSlots()
    {
        for (int x = 0; x < QuickAccessComponent.Max; x++)
        {
            int index = x < 9 ? x + 1 : 0;
            var slot = new Slot(SlotSize, Theme.SlotBackground)
            {
                Position = GetTranslatedPosition(x, 0)
            };
            slot.PrintDigit(index);
            Children.Add(slot);
        }
    }

    void QuickAccess_OnSlotChanged(object? _, InventoryChangedEventArgs e)
    {
        if (e.Index < 0 || e.Index >= Children.Count)
            throw new IndexOutOfRangeException(nameof(e.Index));
        
        if (Children[e.Index] is Slot slot)
        {
            if (e.NewItem is null)
                slot.EraseItem();
            else
                slot.ShowItem(e.NewItem);
        }
    }
}