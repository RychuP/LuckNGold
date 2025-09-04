using LuckNGold.World.Entities.Items.Potions;

namespace LuckNGold.World.Entities.Actors;

internal class Player() : Actor("Player", '@', Color.Yellow, 30, 2, 5, 1000)
{
    public const int FOVRadius = 8;

    public void Reset() =>
        HP = MaxHP;

    public void TryConsumeHealthPotion()
    {
        var potion = Inventory.Items.Where(o => o is HealthPotion p).Cast<HealthPotion>().FirstOrDefault();
        if (potion is not null)
        {
            Inventory.Remove(potion);
            Consume(potion);
        }
        else
            OnFailedAction("You don't have any health potions.");
    }
}