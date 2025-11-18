namespace LuckNGold.Visuals.Consoles;

internal class Inventory : SlotHolder
{
    public Inventory() : base(5, 5, 5)
    {
        for (int y = 0; y < SlotCountY; y++)
        {
            for (int x = 0; x < SlotCountX; x++)
            {
                var slot = new Slot(SlotSize)
                {
                    Position = GetTranslatedPosition(x, y)
                };
                Children.Add(slot);
            }
        }
    }
}