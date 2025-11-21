using LuckNGold.World.Items.Materials;

namespace LuckNGold.Generation.Items.Collectables;

record Coin : Collectable
{
    public Coin(Point position) : base(position, PreciousMetal.Gold)
    { }
}